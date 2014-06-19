using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using TodoistLabelSync;

namespace TodoistLabelSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiToken =  ConfigurationManager.AppSettings["TodoistApiToken"];

            var todoistLabelSync = new TodoistLabelSync(apiToken);
            todoistLabelSync.SyncAllLabels();
        }
    }
}
