using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Core
{
    public class AppConfig
    {
        public DataConfig Data { get; set; } = AppConst.DefaultAppSettings.Data;
        public SecurityConfig Security { get; set; } = AppConst.DefaultAppSettings.Security;
        public EmailConfig Email { get; set; } = AppConst.DefaultAppSettings.Email;
        public LinksConfig Links { get; set; } = AppConst.DefaultAppSettings.Links;
        public FilesConfig Files { get; set; } = AppConst.DefaultAppSettings.Files;
        public ServiceConfig Service { get; set; } = AppConst.DefaultAppSettings.Service;
        public SystemConfig System { get; set; } = AppConst.DefaultAppSettings.System;
        public Dictionary<string, object> OtherSections = new Dictionary<string, object>();

    }

}









