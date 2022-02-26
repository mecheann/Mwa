using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Domain;

namespace Dci.Mnm.Mwa.Infrastructure.Core.Email
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage);
        EmailMessage SetUpEmail(string name, string address, string compiledTemplate, string subject);
    }
}









