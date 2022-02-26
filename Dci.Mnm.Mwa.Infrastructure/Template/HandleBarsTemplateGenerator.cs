
using Dci.Mnm.Mwa.Core;
using HandlebarsDotNet;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Infrastructure.Core;
using System.Diagnostics;

namespace Dci.Mnm.Mwa.Infrastructure
{
    public class HandleBarsTemplateGenerator : ITemplateGenerator
    {
        readonly AppConfig appConfig;
        ILogger<HandleBarsTemplateGenerator> logger;
        IDistributedCache cache;

        static HandleBarsTemplateGenerator()
        {
            HandlebarsHelpers.RegisterHelpers();
        }

        public HandleBarsTemplateGenerator(AppConfig appConfig,
            ILogger<HandleBarsTemplateGenerator> logger,
            IDistributedCache cache)
        {
            this.appConfig = appConfig;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<string> GetCompiledTemplate(string templateName, Object model, string newTemplatePath = null)
        {
            var path = appConfig.Files.EmailTemplatePath;

            if (!String.IsNullOrEmpty(newTemplatePath))
            {
                path = newTemplatePath;
            }
            var baseDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var templatePath = Path.Combine(baseDirectory, path, templateName);

            return await ProcessByHandlebars(model, path, templatePath);
        }

        private async Task<string> ProcessByHandlebars(object model, string path, string templatePath)
        {

            var source = await GetTemplateText(templatePath);
            var templateFunction = Handlebars.Compile(source);

            return templateFunction(model);
        }

        private async Task<string> GetTemplateText(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return await System.IO.File.ReadAllTextAsync(path);
            }
            else if (System.IO.File.Exists(path + ".html"))
            {
                return await System.IO.File.ReadAllTextAsync(path + ".html");
            }
            else
            {
                return await System.IO.File.ReadAllTextAsync(path + ".htm");
            }
        }
    }
}









