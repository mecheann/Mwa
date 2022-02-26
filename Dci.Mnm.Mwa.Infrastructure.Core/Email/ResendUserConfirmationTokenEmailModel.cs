using System;

namespace Dci.Mnm.Mwa.Infrastructure.Core.Email
{
    public class ResendUserConfirmationTokenEmailModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}