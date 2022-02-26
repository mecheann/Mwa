using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Core;

namespace Dci.Mnm.Mwa.Domain
{
    public class EmailMessage : Entity
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
            EmailAttachments = new List<EmailAttachment>();
            Subject = "";
            Content = "";
        }

        //get message subject and body from database
        //addresses are retrieved based on email found for the companies' agent id 

        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> ToCCAddresses { get; set; }
        public List<EmailAddress> ToBCCAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<EmailAttachment> EmailAttachments { get; set; }

        public EmailStatus Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
