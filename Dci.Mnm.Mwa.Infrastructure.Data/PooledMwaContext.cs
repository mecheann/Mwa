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

namespace Dci.Mnm.Mwa.Infrastructure.Data
{
    public class PooledMwaContext : MwaContext
    {
        public PooledMwaContext(DbContextOptions<PooledMwaContext> options)
        : base((DbContextOptions<MwaContext>)(object)options, null, null, null)
        {
            // This does not work and this class can't be unsafe
            // only here if I figure something else out.
            // DO NOT USE


            this.logger = this.GetService<ILogger<PooledMwaContext>>();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.principal = this.GetService<IPrincipal>();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.principal = this.GetService<IPrincipal>();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}










