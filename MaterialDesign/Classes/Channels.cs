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
using System.IO;
namespace MaterialDesign2.Classes
{
    public class Channel
    {
    public string id { get; set; }
    public string author_id { get; set; }
    public string name { get; set; }
    public string rate { get; set; }
    public string rate_count { get; set; }
    public string thumbnail { get; set; }
    public string create_date { get; set; }
    public string update_date { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    }
    public class SendChannel
    {
        public string author_id { get; set; }
        public string name { get; set; }

    }
    class Channels
    {
         public int number_of_try;
        public Channels(){
            number_of_try=0;
        }
        public List<Channel> get_my_channel(string user_id)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "channels/" + user_id;
            List<Channel> channel = new List<Channel>();
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
                                    JToken token = JToken.Parse(data);
                                    if (token["channels"].ToString() != "")
                                    {
                                        int count = token["channels"].Count();
                                        int i = 0;
                                        while (i < count)
                                        {
                                            Channel temp = new Channel();
                                            temp.id = token["channels"][i]["id"].ToString();
                                            temp.thumbnail = token["channels"][i]["thumbnail"].ToString();
                                            temp.name = token["channels"][i]["name"].ToString();
                                            temp.create_date = token["channels"][i]["create_date"].ToString();
                                            temp.rate = token["channels"][i]["rate"].ToString();
                                            //temp.author = JToken.Parse(token["contents"][i]["author"].ToString());
                                            channel.Add(temp);
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
                    //    get_my_channel(user_id);
                    //}
                    //else
                    //{
                        return null;
                   // }
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                }
                return channel;

            }

        }

        public Channel get_channel(string channel_id)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "channel/" + channel_id;

            Channel channel = new Channel();
            using (HttpClient client = new HttpClient())
            {
                try
                {
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
                                    if (token["channel"].ToString() != "")
                                    {
                                        
                                            Channel temp = new Channel();
                                            temp.id = token["channel"]["id"].ToString();
                                            temp.name = token["channel"]["name"].ToString();
                                            temp.author_id = token["channel"]["author_id"].ToString();
                                            temp.thumbnail = token["channel"]["thumbnail"].ToString();
                                            channel=temp;
                                           
                                    }
                                }

                            }
                        }
                    }

                    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
                }
                catch
                {
                    if (number_of_try < 4)
                    {
                        number_of_try += 1;
                        get_channel(channel_id);
                    }
                    else
                    {
                        return null;
                    }
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                }
                return channel;

            }

        }
        public class Follow
        {
           public string user_id;
        }
        public List <Channel> get_followed_channel(string user_id)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "followed_channels" ;
            List<Channel> channels = new List<Channel>();
            Follow f1 = new Follow();
            f1.user_id = user_id;
            
                // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(f1);
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url );
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
                                data = mycontent;
                                if (data != "")
                                {
                                    JToken token = JToken.Parse(data);
                                    if (token["followings"].ToString() != "")
                                    {
                                        int count = token["followings"].Count();
                                        int i = 0;
                                        while (i < count)
                                        {
                                            Channel temp = new Channel();
                                            temp.id = token["followings"][i]["channel_id"].ToString();
                                            temp.thumbnail = token["followings"][i]["thumbnail"].ToString();
                                            temp.name = token["followings"][i]["name"].ToString();
                                            //temp.create_date = token["followings"][i]["create_date"].ToString();
                                            //temp.rate = token["followings"][i]["rate"].ToString();
                                            channels.Add(temp);
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
                //    if (number_of_try < 4)
                //    {
                //        number_of_try += 1;
                //        get_followed_channel(user_id);
                //    }
                    //else
                    //{
                        return null;
                   // }
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                }
                return channels;
        }

        public List<Content> get_channel_contents(string channel_id)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "channel/" + channel_id;

            List<Content> channel = new List<Content>();
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
                                    JToken token = JToken.Parse(data);
                                    JToken author ="";
                                    if (token["channel"].ToString() != "")
                                    {
                                        author = token["channel"]["author"].ToString();
                                        //Channel temp = new Channel();
                                        //temp.id = token["channel"]["id"].ToString();
                                        //temp.name = token["channel"]["name"].ToString();
                                        //temp.author_id = token["channel"]["author_id"].ToString();
                                        //temp.thumbnail = token["channel"]["thumbnail"].ToString();
                                        //channel = temp;
                                    }

                                    
                                    if (token["contents"].ToString() != "")
                                    {
                                        int count = token["contents"].Count();
                                        int i = 0;
                                        while (i < count)
                                        {
                                            Content temp = new Content();
                                            temp.id = token["contents"][i]["id"].ToString();
                                            temp.type = token["contents"][i]["type"].ToString();
                                            temp.length = token["contents"][i]["length"].ToString();
                                            temp.title = token["contents"][i]["title"].ToString();
                                            temp.description = token["contents"][i]["description"].ToString();
                                            temp.author_id = token["contents"][i]["author_id"].ToString();
                                            temp.category_id = token["contents"][i]["category_id"].ToString();
                                            temp.rate = token["contents"][i]["rate"].ToString();
                                            temp.seen_count = token["contents"][i]["seen_count"].ToString();
                                            temp.thumbnail = token["contents"][i]["thumbnail"].ToString();
                                            temp.author = JToken.Parse(author.ToString());//JToken.Parse(token["contents"][i]["author"].ToString());
                                            temp.channel = JToken.Parse(token["channel"].ToString());

                                            channel.Add(temp);
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
                    //    get_my_channel(channel_id);
                    //}
                    //else
                    //{
                        return null;
                    //}
                    // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                }
                return channel;

            }

        }
        public int create_channel(string user_id,string channel_name,string imgpath)
        {
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string url = variables["BaseUrl"] + "create_channel" ;
            {
                //try
                //{
                    HttpClient httpClient = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    //// create channel info
                    var values = new[]{
                        new KeyValuePair<string, string>("name",channel_name),
                        new KeyValuePair<string, string>("author_id",user_id),
                     };
                    foreach (var keyValuePair in values)
                    {
                        form.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }
                    ////end  create channel info

                    //// create image
                    FileStream fs = File.OpenRead(imgpath);
                    var streamContent = new StreamContent(fs);
                    var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(imageContent, "image", Path.GetFileName(imgpath));
                    ////////end create image

                    /////// post request
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = form;
                    var response = httpClient.SendAsync(request).Result;
                    Console.WriteLine(response.ToString());

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


                //}
                //catch
                //{
                //    return 1;
                    //if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true && number_of_try < 6)
                    //{
                    //    number_of_try += 1;
                    //    create_channel(user_id, channel_name,imgpath);
                    //}
                    //else
                    //{
                    //    (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                    //    return 1;
                    //}
               // }
            }
            return 1;
        }
        
        public class Follow_channel
        {
            public string user_id;
            public string target_id;
            public string type;
        }
        public int follow_channel(string targetid,string userid)
        {
            Follow_channel empObj = new Follow_channel();
            empObj.target_id = targetid;
            empObj.type = "channel";
            empObj.user_id =userid;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(empObj);
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                    string url = variables["BaseUrl"] + "follow";
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
                //    follow_channel(targetid, userid);
                //}
                //else
                //{
                    return 1;
                //}
            }
            return 1;
        }
        public int unfollow_channel(string targetid, string userid)
        {
            Follow_channel empObj = new Follow_channel();
            empObj.target_id = targetid;
            empObj.type = "channel";
            empObj.user_id = userid;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(empObj);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                    string url = variables["BaseUrl"] + "unfollow";
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
                //    follow_channel(targetid, userid);
                //}
                //else
                //{
                    return 1;
                //}
            }
            return 1;
        }

    }
}
