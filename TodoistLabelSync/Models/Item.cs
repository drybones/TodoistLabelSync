using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoistLabelSync
{
    public class Item
    {
        public string content { get; set; }
        public int project_id { get; set; }
        public int id { get; set; }
        public List<int> labels { get; set; }
    }
}
