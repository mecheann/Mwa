using Dci.Mnm.Mwa.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Infrastructure.Core.Data
{
    public abstract class BaseMwaContext : IdentityDbContext<User, Role, Guid, UserClaim, IdentityUserRole<Guid>, UserLogin, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public BaseMwaContext(DbContextOptions options)
        : base(options) { }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }

    }
}









