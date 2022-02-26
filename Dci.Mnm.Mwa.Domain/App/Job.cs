using Dci.Mnm.Mwa.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dci.Mnm.Mwa.Domain
{
    public class Job : Entity
    {
        public Job()
        {
            Status = JobStatus.InActive;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Schedule { get; set; }
        public string Configuration { get; set; }
        public string ConfigurationType { get; set; }
        public JobStatus Status { get; set; }
    }
}









