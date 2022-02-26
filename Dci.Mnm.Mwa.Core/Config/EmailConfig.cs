using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Core
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public bool RequireSSL { get; set; }
        public bool CheckServerCertificate { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SenderEmailName { get; set; }
        public string SenderEmailAddress { get; set; }
        public string NotificationEmailAddress { get; set; }
    }
}









