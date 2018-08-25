using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MaterialDesign2.Classes
{
    public class Users
    {
        public int number_of_try;
        public Users()
        {
            number_of_try = 0;
        }
        public JToken get_user_info(string id)
        {
            
            JToken UserInfo=null; 
            using (HttpClient client = new HttpClient())
            {
                //try
                //{
                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)+ "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string url = variables["BaseUrl"] + "user/" + id;
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
                                    if (token["user"].ToString() != "")
                                    {
                                        UserInfo = JToken.Parse(token["user"].ToString());
                                    }
                                }
                            }
                        }
                    }

                    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
                //}
                //catch
                //{
                    //if (number_of_try < 4)
                    //{
                    //    number_of_try += 1;
                    //    get_user_info(id);
                    //}
                    //else
                    //{
                    //    return null;
                    //}
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                //}
            }


            return UserInfo;
        }

        public class Follow_user
        {
            public string user_id;
            public string target_id;
            public string type;
        }

        public int follow_user(string targetid, string userid)
        {
            Follow_user empObj = new Follow_user();
            empObj.target_id = targetid;
            empObj.type = "user";
            empObj.user_id = userid;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(empObj);
            try
            {

                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string url = variables["BaseUrl"] + "follow" ;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
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
                //if (number_of_try < 4)
                //{
                //    number_of_try += 1;
                //    follow_user(targetid, userid);
                //}
                //else
                //{
                    return 1;
                //}
            }
            return 1;
        }

        public List<Content> get_user_contents(string user_id)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "user_contents/" + user_id;
            List<Content> user = new List<Content>();
            using (HttpClient client = new HttpClient())
            {
                //try
                //{
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
                                    if (token["contents"].ToString() != "")
                                    {
                                        int count = token["contents"].Count();
                                        int i = 0;
                                        while (i < count)
                                        {
                                            Content temp = new Content();
                                            temp.id = token["contents"][i]["id"].ToString();
                                            temp.type = token["contents"][i]["type"].ToString();
                                            temp.length = token["contents"][i]["id"].ToString();
                                            temp.title = token["contents"][i]["title"].ToString();
                                            temp.description = token["contents"][i]["description"].ToString();
                                            temp.author_id = token["contents"][i]["author_id"].ToString();
                                            temp.category_id = token["contents"][i]["category_id"].ToString();
                                            temp.rate = token["contents"][i]["rate"].ToString();
                                            temp.seen_count = token["contents"][i]["seen_count"].ToString();
                                            temp.thumbnail = token["contents"][i]["thumbnail"].ToString();
                                            temp.author = JToken.Parse(token["contents"][i]["author"].ToString());

                                            user.Add(temp);
                                            i++;
                                        }

                                    }
                                }

                            }
                        }
                    }

                    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
                //}
                //catch
                //{
                    //if (number_of_try < 4)
                    //{
                    //    number_of_try += 1;
                    //    get_user_contents(user_id);
                    //}
                    //else
                    //{
                    ///    return null;
                    //}
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                //}
                return user;

            }

        }

    }
}
