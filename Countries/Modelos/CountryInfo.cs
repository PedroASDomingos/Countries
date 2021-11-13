using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Modelos
{
    public class CountryInfo
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FoundationYear")]
        public string FoundationYear { get; set; }

        [JsonProperty("CountryHistory")]
        public string CountryHistory { get; set; }

        [JsonProperty("CountryHistoryPT")]
        public string CountryHistoryPT { get; set; }
    }
}
