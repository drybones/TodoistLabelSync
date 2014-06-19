using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TodoistLabelSync
{
    public class TodoistService
    {
        private string apiToken;
        private RestClient client; 

        public TodoistService(string apiToken)
        {
            this.apiToken = apiToken;
            client = new RestClient("https://todoist.com/API/");
        }

        public List<Project> GetProjects()
        {
            var req = new RestRequest("getProjects", Method.GET);
            req.AddParameter("token", apiToken);

            var resp = client.Execute<List<Project>>(req);
            return resp.Data;
        }

        public Dictionary<string,Label> GetLabels()
        {
            var req = new RestRequest("getLabels", Method.GET);
            req.AddParameter("token", apiToken);

            var resp = client.Execute<Dictionary<string, Label>>(req);
            return resp.Data;
        }

        public Label AddLabel(string name)
        {
            var req = new RestRequest("addLabel", Method.POST);
            req.AddParameter("token", apiToken);
            req.AddParameter("name", name);

            var resp = client.Execute<Label>(req);
            return resp.Data;
        }

        public bool DeleteLabel(string name)
        {
            var req = new RestRequest("deleteLabel", Method.POST);
            req.AddParameter("token", apiToken);
            req.AddParameter("name", name);

            var resp = client.Execute(req);
            return (resp.StatusCode == System.Net.HttpStatusCode.OK);
        }
        
        public List<Item> GetUncompletedItems(int project_id)
        {
            var req = new RestRequest("getUncompletedItems", Method.GET);
            req.AddParameter("token", apiToken);
            req.AddParameter("project_id", project_id);

            var resp = client.Execute<List<Item>>(req);
            return resp.Data;
        }

        public Item UpdateItemLabels(Item item)
        {
            var req = new RestRequest("updateItem", Method.POST);
            req.AddParameter("token", apiToken);
            req.AddParameter("id", item.id);
            req.AddParameter("labels", "["+string.Join(",",item.labels.ToArray())+"]");

            var resp = client.Execute<Item>(req);
            return resp.Data;
        }
    }
}
