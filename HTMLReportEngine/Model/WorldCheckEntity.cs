using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReportEngine.Model
{
    internal class WorldCheckEntity
    {
        public string U_ID { get; set; }
        public string CODE { get; set; }
        public string FULL_NAME { get; set; }
        public double? SCORE { get; set; }
        public string CITIZENSHIP { get; set; }
        public object DOB { get; set; }
        public string INFERRED_DOB { get; set; }
        public string CATEGORY { get; set; }
        public string KEYWORD { get; set; }
        public string ADDRESS { get; set; }
    }
}
