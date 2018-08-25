using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Net;
namespace MaterialDesign2.Classes
{
    public class CommentClass
    {
        public string user_id;
        public string content_id;
        public string body;
        public string answer;
        public JToken username;
    }
    class Commentjson
    {
        public string user_id;
        public string content_id;
        public string body;
    }
    public class Comment
    {
        public int number_of_try;
        public Comment()
        {
            number_of_try = 0;
        }
        public int Post_Comment(string Cid,string Uid,string Cbody)
        {
            Commentjson empObj = new Commentjson();
            empObj.body = Cbody;
            empObj.content_id = Cid;
            empObj.user_id = Uid;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(empObj);
            try
            {
                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string url = variables["BaseUrl"] + "comment";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,url );
                    request.Content = new StringContent(jsonData,
                                                        Encoding.UTF8,
                                                        "application/json");
                    request.Version = HttpVersion.Version10;
                    using (HttpResponseMessage response = client.SendAsync(request).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string mycontent = content.ReadAsStringAsync().Result;
                            string data = string.Empty;
                            {
                                data = mycontent;
                                if (data != "")
                                {
                                    JToken token = JToken.Parse(data);
                                    if (token["error"].ToString() == "False")
                                    {
                                        return 0;
                                    }
                                    else
                                    {
                                        return -1;
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            catch
            {
                //if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true && number_of_try < 6)
                //{
                //    number_of_try += 1;
                //    Post_Comment( Cid, Uid, Cbody);
                //}
                //else
                //{
                    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                    return 1;
               // }
            }
            return 1;
        }
        public List<CommentClass> Get_Comments(string id)
        {
            List<CommentClass> CommentList = new List<CommentClass>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                    string url = variables["BaseUrl"] + "comments/" + id;

                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string mycontent = content.ReadAsStringAsync().Result;
                            string data = string.Empty;
                            {
                                data = mycontent;
                                if (data != "")
                                {
                                    JToken token = JToken.Parse(data);
                                    if (token["comments"].ToString() != "")
                                    {
                                        int count = token["comments"].Count();
                                        int i=0;
                                        while (i<count)
                                        {
                                            CommentClass temp = new CommentClass();
                                            Users tempuser = new Users();
                                            temp.content_id = token["comments"][i]["content_id"].ToString();
                                            temp.body = token["comments"][i]["body"].ToString();
                                            temp.answer = token["comments"][i]["answer"].ToString();
                                            temp.user_id = token["comments"][i]["user_id"].ToString();
                                            temp.username = tempuser.get_user_info(temp.user_id);
                                            CommentList.Add(temp);
                                            i++;
                                        }         
                                    }
                                }
                                
                            }
                        }
                    }

                    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
                }
                catch
                {
                    //if (number_of_try < 4)
                    //{
                    //    number_of_try += 1;
                    //    Get_Comments(id);
                    //}
                    //else
                    //{
                        return null;
                   // }
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                }
            }
        

            return CommentList;
        }
    }
}
