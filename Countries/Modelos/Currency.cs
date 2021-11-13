using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Modelos
{
    public class Currency : Country
    {
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }
}
