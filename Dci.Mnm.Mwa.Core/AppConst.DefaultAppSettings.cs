using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core
{
    public partial class AppConst
    {
        public static class DefaultAppSettings
        {
            public static DataConfig Data = new DataConfig
            {
                RunMigrationOnStartup = false,
            };

            public static SecurityConfig Security = new SecurityConfig
            {
                Data = new SecurityDataConfig
                {
                    DefaultRootAdminRole = "RootAdmin",
                    DefaultRootAdminUserName = "Admin",
                    CreateDefaultRootAdmin = true,
                    DeleteOldActionOnStartup = true,
                    DefaultRootAdminPassword = "p@ssword1",
                    DefaultRootAdminUserEmail = "Admin@localhost",
                    DefaultUserRole = "User",
                },
                Token = new TokenConfig
                {
                    SecretKey = "3843439DDF!E0394334",
                    Issuer = "https://dci.mnm.mwa/2019/Dap",
                    Audience = "app",
                    TokenLifeTimeInMinutes = 10,
                    HeaderPayloadCookieName = "ahpc",
                    SignatureCookieName = "asc",

                }

            };

            public static EmailConfig Email = new EmailConfig
            {
                SmtpServer = "localhost",
                SmtpPort = 25,
                CheckServerCertificate = false,
                RequireSSL = false,
                SmtpUsername = "",
                SmtpPassword = "",
                SenderEmailAddress = "mwa@mailinator.com",
                SenderEmailName = "My Word App (MWA)",

            };

            public static LinksConfig Links = new LinksConfig
            {
                BaseUrl = "https://localhost/",
                FileUrl = "api/files/{0}",
                ImagesBaseUrl = "/wwwroot",
                RequestResetUserPasswordLink = "/reset-password?token={0}&email={1}",
                ConfirmUserLink = "/confirm-user?token={0}&email={1}"

            };

            public static FilesConfig Files = new FilesConfig
            {
                htmlToPdfPath = "HtmlToPdf/tools/wkhtmltopdf.exe",
                filePath = "../files",
                PdfTemplatePath = "./Reports/Templates/",
                CreateFolderIfDoesExist = true,
                EmailTemplatePath = "./Email/Templates/",
            };

            public static ServiceConfig Service = new ServiceConfig
            {
                MaxRetry = 1
            };

            public static SystemConfig System = new SystemConfig
            {
                General = new GeneralConfig
                {
                    DateFormat = "M/dd/yyyy",
                    DateTimeFormat = "yyyy-MM-dd hh:mm:ss tt",
                }
            };

        }
    }
}
