using Dci.Mnm.Mwa.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using Dci.Mnm.Mwa.Infrastructure.Core.Data;

namespace Dci.Mnm.Mwa.Infrastructure.Data
{
    class MwaContextFactory : IDesignTimeDbContextFactory<MwaContext>
    {
        public MwaContext CreateDbContext(string[] args)
        {
            Console.WriteLine("-----IN Database Factory---");
            var configBuilder = new ConfigurationBuilder();
            Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
            var configuration = configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.{Environment}.json", true, true)
                .AddEnvironmentVariables().Build();

            var appConfigSection = configuration.GetSection(nameof(AppConfig));
            var appConfig = appConfigSection.GetChildren() as AppConfig;
            var optionsBuilder = new DbContextOptionsBuilder<MwaContext>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var options = optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default")).Options;
            var nullLogger = new NullLogger<MwaContext>();
            var context = new MwaContext(options, nullLogger, principal, appConfig.Data);
            return context;

        }
    }
}









