using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Core.Database;
using Dci.Mnm.Mwa.Domain;
using Dci.Mnm.Mwa.Infrastructure.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using ClosedXML.Excel;

namespace Dci.Mnm.Mwa.Infrastructure.Data
{
    public class MwaContext : BaseMwaContext, DatabaseWithSeeding
    {
        protected ILogger<MwaContext> logger;
        protected IPrincipal principal;
        protected DataConfig dataConfig;
        public MwaContext(DbContextOptions<MwaContext> options, ILogger<MwaContext> logger, IPrincipal principal, DataConfig dataConfig)
        : base(options)
        {
            this.logger = logger;
            this.principal = principal;
            this.dataConfig = dataConfig;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<UserClaim>().ToTable("UserClaim");
            builder.Entity<UserLogin>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");

            // will search for mappings and find them in specified assembly
            builder.ApplyConfigurationsFromAssembly(typeof(MwaContext).Assembly);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // Don't pluralize names
                if (!entityType.DisplayName().Contains("<"))
                {
                    entityType.SetTableName(entityType.DisplayName());
                }

                //OneToMany Cascade Delete Convention
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        }

        public void OnBeforeSaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            // auditingService.AddAuditDetails(this.ChangeTracker.Entries<Entity>());

            // //auditingService.AddAuditDetails(ChangeTracker.Entries<Entity>() as IEnumerable<EntityEntry<Entity>>);
            // //TODO: Fixed auditing Delete and Add
            // //auditingService.AddAuditDetails(this.GetDeletedRelationships(), AuditEvent.Deleted);
            // //auditingService.AddAuditDetails(this.GetAddedRelationships(), AuditEvent.Added);
            AddTimestamps();
        }

        protected void AddTimestamps()
        {
            try
            {
                var entities = ChangeTracker.Entries().Where(x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
                var now = DateTimeOffset.Now;
                var currentUsername = principal.UserName() ?? "System";
                var currentUserId = principal.UserId() ?? Guid.Empty;

                foreach (var entity in entities)
                {

                    if (entity.State == EntityState.Added)
                    {
                        ((IEntity)entity.Entity).CreationDate = now;
                        ((IEntity)entity.Entity).CreatedById = currentUserId;
                        ((IEntity)entity.Entity).CreatedBy = currentUsername;
                    }

                    ((IEntity)entity.Entity).ModificationDate = now;
                    ((IEntity)entity.Entity).ModifiedBy = currentUsername;
                    ((IEntity)entity.Entity).ModifiedById = currentUserId;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Failed to add timestamp with error: {errorMessage}", ex.GetInnerMessages());
            }
        }

        public async Task Seed(AppConfig appConfig)
        {
            SeedSecurityData(appConfig);

            await SeedJobs(appConfig);
        }

        public void SeedSecurityData(AppConfig appConfig)
        {
            try
            {

                // Get operations and store all operations
                var allActions = AppConst.Operations.GetAll();
                logger.LogInformation($"System Operations:{allActions.Count()}");
                var dbOperations = this.Operations.ToList();
                logger.LogInformation($"DB Operations:{dbOperations.Count()}");

                if (appConfig.Security.Data.DeleteOldActionOnStartup)
                {
                    logger.LogInformation($"Refreshing DB Operations");
                    this.Operations.RemoveRange(dbOperations);

                    allActions.ForEach(x => this.Operations.Add(new Operation { Name = x }));
                    this.SaveChanges();
                    dbOperations = this.Operations.ToList();
                }

                var newOperations = allActions.Where(x => !dbOperations.Any(y => x == y.Name)).Select(x => new Operation { Name = x });
                this.Operations.AddRange(newOperations);
                this.SaveChanges();

                // Create Admin role
                var adminRoleName = appConfig.Security.Data.DefaultRootAdminRole;
                logger.LogInformation($"attempting admin role: {adminRoleName}");

                var adminRole = this.Roles.FirstOrDefault(x => x.Name == adminRoleName);

                if (adminRole == null)
                {
                    logger.LogInformation("Role doesn't exist, creating it...");
                    adminRole = new Role
                    {
                        Id = Utility.CreateNewIdFromString(adminRoleName),
                        Name = adminRoleName,
                        Description = "Root Admin Role",
                        NormalizedName = adminRoleName.ToUpper(),
                    };

                    this.Roles.Add(adminRole);
                    this.SaveChanges();
                    logger.LogInformation($"Created {adminRoleName} Role.");
                }

                // add claims to admin role
                logger.LogInformation("Updating Admin Role Operations Claims");
                var roleClaims = this.RoleClaims.Where(x => x.RoleId == adminRole.Id && x.ClaimType == AppConst.ClaimTypes.Operation);
                this.RoleClaims.RemoveRange(roleClaims);
                this.SaveChanges();
                logger.LogInformation("Removed all admin role Operations Claims");

                var random = new System.Random(1);
                var newRoleClaims = allActions.Select(claimValue => new IdentityRoleClaim<Guid>
                {
                    RoleId = adminRole.Id,
                    ClaimType = AppConst.ClaimTypes.Operation,
                    ClaimValue = claimValue
                });

                this.RoleClaims.AddRange(newRoleClaims);
                this.SaveChanges();
                logger.LogInformation("Updated Admin Role Operations Claims");

                // Create Admin user
                logger.LogInformation("Creating user. Note this will only be done if there are no users in the system");

                var adminUserName = appConfig.Security.Data.DefaultRootAdminUserName;
                var adminUser = this.Users.FirstOrDefault(x => x.UserName == adminUserName);

                if (!this.Users.Any() && adminUser == null)
                {

                    var passwordHasher = new PasswordHasher<User>();
                    var adminEmail = appConfig.Security.Data.DefaultRootAdminUserEmail;
                    adminUser = new User
                    {
                        Id = Utility.CreateNewIdFromString(adminUserName),
                        UserName = adminUserName,
                        FirstName = adminUserName,
                        LastName = "Admin",
                        AccessFailedCount = 0,
                        PhoneNumber = "12345678910",
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedUserName = adminUserName.ToUpper(),
                        NormalizedEmail = adminEmail.ToUpper(),
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Active = UserStatus.Active
                    };
                    adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, appConfig.Security.Data.DefaultRootAdminPassword);

                    this.Users.Add(adminUser);
                    this.SaveChanges();

                    // Add user to role
                    var adminUserRole = new IdentityUserRole<Guid>
                    {

                        RoleId = adminRole.Id,
                        UserId = adminUser.Id
                    };

                    this.UserRoles.Add(adminUserRole);
                    this.SaveChanges();
                }

                // Create User Role
                var userRoleName = appConfig.Security.Data.DefaultUserRole;
                logger.LogInformation("attempting user role: {roleName}", userRoleName);

                var userRole = this.Roles.FirstOrDefault(x => x.Name == userRoleName);

                if (userRole == null)
                {
                    logger.LogInformation("Role doesn't exist, creating it...");
                    userRole = new Role
                    {
                        Id = Utility.CreateNewIdFromString(userRoleName),
                        Name = userRoleName,
                        Description = "User Role",
                        NormalizedName = userRoleName.ToUpper(),
                    };

                    this.Roles.Add(userRole);
                    this.SaveChanges();
                    logger.LogInformation("Created {roleName} Role.", userRoleName);

                    var userRoleClaim = new IdentityRoleClaim<Guid>
                    {
                        RoleId = userRole.Id,
                        ClaimType = AppConst.ClaimTypes.Operation,
                        ClaimValue = AppConst.Operations.IS_AUTENTICATED,
                    };

                    this.RoleClaims.Add(userRoleClaim);

                    // userRoleClaim = new IdentityRoleClaim<Guid>
                    // {
                    //     RoleId = userRole.Id,
                    //     ClaimType = AppConst.ClaimTypes.Operation,
                    //     ClaimValue = AppConst.Operations.VIEW_TRACKED_BOL,
                    // };
                    // this.RoleClaims.Add(userRoleClaim);

                    // userRoleClaim = new IdentityRoleClaim<Guid>
                    // {
                    //     RoleId = userRole.Id,
                    //     ClaimType = AppConst.ClaimTypes.Operation,
                    //     ClaimValue = AppConst.Operations.EDIT_TRACKED_BOL,
                    // };
                    this.RoleClaims.Add(userRoleClaim);

                    this.SaveChanges();
                }


            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error while trying to Seed Security Data:{errorMessage}", ex.GetInnerMessages());
            }
        }

        public async Task SeedJobs(AppConfig appConfig)
        {
            try
            {
                if (!Jobs.Any(x => x.Type == "Dci.Mnm.Mwa.Service.Jobs.EmailJob"))
                {
                    var emailJob = new Job();
                    emailJob.Id = Utility.CreateNewIdFromString("Email Job");

                    emailJob.Name = "Email Job";
                    emailJob.Type = "Dci.Mnm.Mwa.Service.Jobs.EmailJob";
                    emailJob.Status = JobStatus.Active;
                    emailJob.Schedule = "0 * * ? * *";

                    Jobs.Add(emailJob);
                    SaveChanges();
                }

                if (!Jobs.Any(x => x.Type == "Dci.Mnm.Mwa.Service.Jobs.QueueAllMonthlyStatementEmailJob"))
                {
                    var QueueAllMonthlyStatementEmailJob = new Job();
                    QueueAllMonthlyStatementEmailJob.Id = Utility.CreateNewIdFromString("QueueAllMonthlyStatementEmailJob");

                    QueueAllMonthlyStatementEmailJob.Name = "QueueAllMonthlyStatementEmail Job";
                    QueueAllMonthlyStatementEmailJob.Type = "Dci.Mnm.Mwa.Service.Jobs.QueueAllMonthlyStatementEmailJob";
                    QueueAllMonthlyStatementEmailJob.Status = JobStatus.Active;
                    QueueAllMonthlyStatementEmailJob.Schedule = "0 * * ? * *";

                    Jobs.Add(QueueAllMonthlyStatementEmailJob);
                    SaveChanges();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ex in seeing: {message}", ex.GetInnerMessages());
            }
        }

     }
}










