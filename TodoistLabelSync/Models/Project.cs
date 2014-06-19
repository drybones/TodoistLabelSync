using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TodoistLabelSync
{
    public class Project
    {
        public string name { get; set; }
        public string color { get; set; }
        public int item_order { get; set; }
        public int indent { get; set; }
        public int id { get; set; }

        public string labelName { get { return Regex.Replace(name, @"\W", ""); } }
    }
}
