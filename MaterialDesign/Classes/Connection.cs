using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Newtonsoft.Json.Linq;
using System.Net.Http;
using SharpRaven;
using SharpRaven.Data;

namespace MaterialDesign2.Classes
{
    public class Connection
    {
        public JToken response_content;
        public string url;

        public void get_connection()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string mycontent = content.ReadAsStringAsync().Result;
                            int index = mycontent.LastIndexOf("<link");
                            if (index > 0)
                                mycontent = mycontent.Substring(0, index);
                            string data = string.Empty;
                            {
                                data = mycontent;
                                if (data != "")
                                {
                                    response_content = null;
                                    JToken token = JToken.Parse(data);
                                    try
                                    {
                                     if (token["contents"].ToString() != "")
                                    {
                                        
                                        response_content = token;
                                    }
                                    }
                                    catch
                                    {
                                         if(token["content"].ToString() != "")
                                        {
                                            response_content = token;
                                        }
                                    }
                                    
                                }

                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                    string key = variables["SentryKey"];
                    string project = variables["SentryProject"];
                    var ravenClient = new RavenClient("https://" + key + "@sentry.io/" + project);
                    ravenClient.Capture(new SentryEvent(exception));
                    return;
                }
            }
        }

    }
}
