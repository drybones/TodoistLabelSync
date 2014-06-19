using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TodoistLabelSync
{
    public class TodoistLabelSync
    {
        private string apiToken;

        public TodoistLabelSync(string apiToken)
        {
            this.apiToken = apiToken;
        }

        public void SyncAllLabels()
        {
            var svc = new TodoistService(apiToken);
            var projectTreeLabels = new Dictionary<int, string>();

            var projects = svc.GetProjects();
            var labels = svc.GetLabels();

            var requiredProjectLabels = from p in projects select p.labelName;
            var labelsToDelete = from l in labels where !requiredProjectLabels.Contains(l.Value.name) select l;

            Trace.WriteLine("Project labels required: " + String.Join(", ",requiredProjectLabels));
            Trace.WriteLine("Current labels to delete: " + String.Join(", ",from l in labelsToDelete select l.Value.name));

            
            foreach(var l in labelsToDelete)
            {
                Trace.TraceInformation("Deleting label: " + l.Value.name + " [" + l.Value.id + "]");
                svc.DeleteLabel(l.Value.name);
                // Don't remove the old label -- it's not important and we don't want 
                // the collection to change while enumertaing it.
            }
            var labelsToAdd = from l in requiredProjectLabels where !(from ll in labels select ll.Value.name).Contains(l) select l;
            foreach (var l in labelsToAdd)
            {
                Trace.TraceInformation("Creating label: " + l);
                var label = svc.AddLabel(l);
                labels.Add(label.name.ToLower(), label);
            }

            foreach (var p in projects)
            {
                projectTreeLabels[p.indent] = p.labelName;

                Trace.WriteLine(new String('>', p.indent) + " PROJECT: " + p.name);
                var items = svc.GetUncompletedItems(p.id);
                foreach (var i in items)
                {
                    var requiredItemLabels = from l in projectTreeLabels where l.Key <= p.indent orderby l.Value select l.Value;
                    var currentItemLabels = from cl in i.labels join l in labels on cl equals l.Value.id orderby l.Value.name select l.Value.name;
                    Trace.WriteLine(new String('-', p.indent + 1) + " ITEM: " + i.content);

                    if (!requiredItemLabels.SequenceEqual(currentItemLabels))
                    {
                        Trace.TraceInformation("Updating item: " + i.content);
                        Trace.TraceInformation("  currently: " + string.Join(", ", currentItemLabels));
                        Trace.TraceInformation("  should be: " + string.Join(", ", requiredItemLabels));
                        i.labels = (from l in labels where requiredItemLabels.Contains(l.Value.name) select l.Value.id).ToList();
                        svc.UpdateItemLabels(i);
                    }
                }
            }
        }
    }
}
