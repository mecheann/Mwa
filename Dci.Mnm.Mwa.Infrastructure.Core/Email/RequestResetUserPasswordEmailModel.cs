using System;

namespace Dci.Mnm.Mwa.Infrastructure.Core.Email
{
    public class RequestResetUserPasswordEmailModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string ImageBaseUrl { get; set; }
    }
}