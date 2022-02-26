using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Dci.Mnm.Mwa.Core
{
    public class Seeding
    {
        public Seeding()
        {
            this.Data = new List<JObject>();
        }
        public string ContextName { get; set; }
        public string EntityName { get; set; }
        public SeedingIdGenerationType SeedingIdGenerationType { get; set; }
        public Boolean OverrideExisting { get; set; }

        public string CompareField { get; set; }

        public string DataFilePath { get; set; }
        public List<JObject> Data { get; set; }

    }
}








