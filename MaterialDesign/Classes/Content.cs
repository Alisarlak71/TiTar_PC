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
using SharpRaven.Data;
using SharpRaven;

namespace MaterialDesign2.Classes
{
    public class Content
    {
        public string id { get; set; }
        public string type_id { get; set; }
        public string subtitle_id { get; set; }
        public string category_id { get; set; }
        public string author_id { get; set; }
        public string uid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string thumbnail { get; set; }
        public string rate { get; set; }
        public string rate_count { get; set; }
        public string seen_count { get; set; }
        public string download_count { get; set; }
        public string sell { get; set; }
        public string size { get; set; }
        public string length { get; set; }
        public string requirements { get; set; }
        public string active { get; set; }
        public string confirm { get; set; }
        public string version_name { get; set; }
        public string version { get; set; }
        public string language { get; set; }
        public string package_name { get; set; }
        public string file_name { get; set; }
        public string slug { get; set; }
        public string file_type { get; set; }
        public string has_subtitle { get; set; }
        public string encoder { get; set; }
        public string price { get; set; }
        public string off { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
        public string create_date { get; set; }
        public string update_date { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string channel_id { get; set; }
        public bool rated { get; set; }
        public JToken author { get; set; }
        public JToken channel { get; set; }
        public JToken related { get; set; }
        public List<MaterialDesign2.Classes.CommentClass> comments { get; set; }
        public MaterialDesign2.Classes.Channel channel_of_single { get; set; }
       public Content()
        {
            comments = new List<CommentClass>();
            channel_of_single = new MaterialDesign2.Classes.Channel();
        }
    }

  

    public class ReciveContent
    {
        public int number_of_try;
        public ReciveContent()
        {
            number_of_try = 0;
        }
        //public void recive_content (string type,string ContentType)
        //{
        //    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
        //    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            
        //    switch (type)
        //    {
        //        case "last_video":
        //            {
        //                number_of_try = 0;
        //                string url = variables["BaseUrl"] + "contents/1/";
        //                connect_to_server_get_content(url + ContentType, type);
        //                break;
        //            }
        //        case "most_seen":
        //            {
        //                number_of_try = 0;
        //                string url = variables["BaseUrl"] + "contents/2/";
        //                connect_to_server_get_content(url + ContentType , type);
        //                break;
        //            }
        //        case "most_love":
        //            {
        //                number_of_try = 0;
        //                string url = variables["BaseUrl"] + "contents/3/";
        //                connect_to_server_get_content(url + ContentType , type);
        //                break;
        //            }
        //        case "most_down":
        //            {
        //                number_of_try = 0;
        //                string url = variables["BaseUrl"] + "contents/4/";
        //                connect_to_server_get_content(url + ContentType , type);
        //                break;
        //            }
        //        case "most_sell":
        //            {
        //                number_of_try = 0;
        //                string url = variables["BaseUrl"] + "contents/5/";
        //                connect_to_server_get_content(url + ContentType, type);
        //                break;
        //            }
        //    }
        //}

        //public void connect_to_server_get_content(string url, string type)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            using (HttpResponseMessage response = client.GetAsync(url).Result)
        //            {
        //                using (HttpContent content = response.Content)
        //                {
        //                    string mycontent = content.ReadAsStringAsync().Result;
        //                    int index = mycontent.LastIndexOf("<link");
        //                    if (index > 0)
        //                        mycontent = mycontent.Substring(0, index);
        //                    string data = string.Empty;
        //                    {
        //                        data = mycontent;
        //                        if (data != "")
        //                        {
        //                            JToken token = JToken.Parse(data);
        //                            if (token["contents"].ToString() != "")
        //                            {
        //                                int count = token["contents"].Count();
        //                                int i = 0;
        //                                List<Content> classdata = new List<Content>();
        //                                while (i < count)
        //                                {
        //                                    Content temp = new Content();
        //                                    temp.id = token["contents"][i]["id"].ToString();
        //                                    temp.type = token["contents"][i]["type"].ToString();

        //                                    temp.length = token["contents"][i]["id"].ToString();
        //                                    temp.title = token["contents"][i]["title"].ToString();
        //                                    temp.description = token["contents"][i]["description"].ToString();
        //                                    temp.author_id = token["contents"][i]["author_id"].ToString();
        //                                    temp.category_id = token["contents"][i]["category_id"].ToString();
        //                                    temp.rate = token["contents"][i]["rate"].ToString();
        //                                    temp.seen_count = token["contents"][i]["seen_count"].ToString();
        //                                    temp.thumbnail = token["contents"][i]["thumbnail"].ToString();
        //                                    temp.file_name = token["contents"][i]["file_name"].ToString();
        //                                    temp.channel_id = token["contents"][i]["channel_id"].ToString();
        //                                    temp.author = JToken.Parse(token["contents"][i]["author"].ToString());
        //                                    classdata.Add(temp);

        //                                    i++;
        //                                }

        //                                switch (type)
        //                                {
        //                                    //case "last_video":
        //                                    //    (MaterialDesign.App.Current as MaterialDesign.App).LasteContent = (classdata);
        //                                    //    break;
        //                                    //case "most_seen":
        //                                    //    (MaterialDesign.App.Current as MaterialDesign.App).MostSeen = (classdata);
        //                                    //    break;
        //                                    //case "most_love":
        //                                    //    (MaterialDesign.App.Current as MaterialDesign.App).MostLove = (classdata);
        //                                    //    break;
        //                                    //case "most_down":
        //                                    //    (MaterialDesign.App.Current as MaterialDesign.App).MostDown = (classdata);
        //                                    //    break;
        //                                    //case "most_sell":
        //                                    //    (MaterialDesign.App.Current as MaterialDesign.App).MostSell = (classdata);
        //                                    //    break;

        //                                }

        //                            }
        //                        }

        //                    }
        //                }
        //            }

        //            (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
        //        }
        //        catch (Exception exception)
        //        {
        //            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
        //            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
        //            string key = variables["SentryKey"];
        //            string project = variables["SentryProject"];
        //            var ravenClient = new RavenClient("https://" + key + "@sentry.io/" + project);
        //            ravenClient.Capture(new SentryEvent(exception));
        //            //if (number_of_try < 4)
        //            //{
        //            //   // number_of_try += 1;
        //            //    //connect_to_server_get_content(url, type);
        //            //}
        //            //else
        //            //{
        //            return;
        //            //}
        //            // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
        //        }
        //    }
        //}
        //public void get_search_content(string query )
        //{
        //    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
        //    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
        //    string url = variables["BaseUrl"] + "search/" + query;

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            using (HttpResponseMessage response = client.GetAsync(url).Result)
        //            {
        //                using (HttpContent content = response.Content)
        //                {
        //                    string mycontent = content.ReadAsStringAsync().Result;
        //                    int index = mycontent.LastIndexOf("<link");
        //                    if (index > 0)
        //                        mycontent = mycontent.Substring(0, index);
        //                    string data = string.Empty;
        //                    {
        //                        data = mycontent;
        //                        if (data != "")
        //                        {
        //                            JToken token = JToken.Parse(data);
        //                            if (token["contents"].ToString() != "")
        //                            {
        //                                int count = token["contents"].Count();
        //                                int i = 0;
        //                                List<Content> classdata = new List<Content>();
        //                                while (i < count)
        //                                {
        //                                    Content temp = new Content();
        //                                    temp.id = token["contents"][i]["id"].ToString();
        //                                    temp.type = token["contents"][i]["type"].ToString();
        //                                    temp.length = token["contents"][i]["length"].ToString();
        //                                    temp.title = token["contents"][i]["title"].ToString();
        //                                    temp.description = token["contents"][i]["description"].ToString();
        //                                    temp.author_id = token["contents"][i]["author_id"].ToString();
        //                                    temp.category_id = token["contents"][i]["category_id"].ToString();
        //                                    temp.rate = token["contents"][i]["rate"].ToString();
        //                                    temp.seen_count = token["contents"][i]["seen_count"].ToString();
        //                                    temp.thumbnail = token["contents"][i]["thumbnail"].ToString();
        //                                    temp.channel_id = token["contents"][i]["channel_id"].ToString();

        //                                    temp.author = JToken.Parse(token["contents"][i]["author"].ToString());
        //                                    classdata.Add(temp);
        //                                    i++;
        //                                }
        //                                (MaterialDesign.App.Current as MaterialDesign.App).SearchResult = classdata;
                                        
        //                            }
        //                        }

        //                    }
        //                }
        //            }

        //            (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
        //        }
        //        catch
        //        {
        //            if (number_of_try < 4)
        //            {
        //                number_of_try += 1;
        //                get_search_content(query);
        //            }
        //            else
        //            {
        //                return;
        //            }
        //            // (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
        //        }
        //    }
        //}

        //public Content get_single_content(string id)
        //{
        //    Content classdata = new Content();
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
        //            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
        //            string url = variables["BaseUrl"] + "content/" + id + "/" + (MaterialDesign.App.Current as MaterialDesign.App).User.id;

        //            HttpResponseMessage response = client.GetAsync(url).Result;
                    
        //                HttpContent content = response.Content;
                        
        //                    string mycontent = content.ReadAsStringAsync().Result;
        //            int index = mycontent.LastIndexOf("<link");
        //            if (index > 0)
        //                mycontent = mycontent.Substring(0, index);
        //            string data = string.Empty;
        //                    {
        //                        data = mycontent;
        //                        if (data != "")
        //                        {
        //                            JToken token = JToken.Parse(data);
        //                            if (token["content"].ToString() != "")
        //                            {
        //                                classdata.id = token["content"]["id"].ToString();
        //                                classdata.length = token["content"]["length"].ToString();
        //                                classdata.title = token["content"]["title"].ToString();
        //                                classdata.description = token["content"]["description"].ToString();
        //                                classdata.author_id = token["content"]["author_id"].ToString();
        //                                classdata.category_id = token["content"]["category_id"].ToString();
        //                                classdata.rate = token["content"]["rate"].ToString();
        //                                classdata.seen_count = token["content"]["seen_count"].ToString();
        //                                classdata.thumbnail = token["content"]["thumbnail"].ToString();
        //                                classdata.file_name = token["content"]["file_name"].ToString();
        //                                classdata.channel_id = token["content"]["channel_id"].ToString();
        //                                classdata.type=token["content"]["type"].ToString();
        //                                classdata.length = token["content"]["length"].ToString();

        //                                classdata.author = JToken.Parse(token["content"]["author"].ToString());

        //                                JToken channel = token["content"]["channel"].ToString();
        //                                if (channel.ToString() != ""&&classdata.type=="video")
        //                                {

        //                                    Channel temp = new Channel();
        //                                    temp.id = token["content"]["channel"][0]["id"].ToString();
        //                                    temp.name = token["content"]["channel"][0]["name"].ToString();
        //                                    temp.author_id = token["content"]["channel"][0]["author_id"].ToString();
        //                                    temp.thumbnail = token["content"]["channel"][0]["thumbnail"].ToString();
        //                                    classdata.channel_of_single = temp;

        //                                }

        //                                classdata.rated = (bool)token["content"]["rated"];
        //                                data = token["content"]["comments"].ToString(); 
        //                                if (data != "")
        //                                {
        //                                    JToken comments = JToken.Parse(data);
        //                                    if (comments.ToString() != "")
        //                                    {
        //                                        int count = comments.Count();
        //                                        int i = 0;
        //                                        while (i < count)
        //                                        {
        //                                            CommentClass temp = new CommentClass();

        //                                            Users tempuser = new Users();
        //                                            temp.content_id = comments[i]["content_id"].ToString();
        //                                            temp.body = comments[i]["body"].ToString();
        //                                            temp.answer = comments[i]["answer"].ToString();
        //                                            temp.user_id = comments[i]["user_id"].ToString();
        //                                            temp.username = tempuser.get_user_info(temp.user_id);
        //                                            classdata.comments.Add(temp);
        //                                            i++;
        //                                        }
        //                                    }
        //                                }
        //                                classdata.related = JToken.Parse(token["content"]["related"].ToString());
        //                            }
        //                        }
        //                    }
        //            (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = true;
        //        }
        //        catch
        //        {
                    
        //                return null;
                    
        //        }
        //    }
        //    return classdata;
        //}
    }

    public class SendContent
    {
        public int number_of_try;
        public SendContent()
        {
            number_of_try = 0;
        }
        public class jsonSendContent
        {
            public string author_id { get; set; }
            public string channel_id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string age { get; set; }
            public string tags { get; set; }
            public string category_id { get; set; }

            public string price { get; set; }
            public string extension { get; set; }
            public string size { get; set; }
            public string length { get; set; }
            public string language { get; set; }
        }
        public string send_content_video( string authorid,string channelid,string title,string description,string age,string language, string tags,string price,string extension,string size,string length )
        {
            string file_name = "";
            jsonSendContent empObj = new jsonSendContent();
                empObj.author_id = authorid ;
                empObj.channel_id = channelid;
                empObj.title = title;
                empObj.description = description;
                empObj.age = age;
                empObj.language = language;
                empObj.tags = tags;
                empObj.price = price;
                empObj.extension = extension;
                empObj.size = size;
                empObj.length = length;
                empObj.category_id = "1";
                // Convert Employee object to JOSN string format   
                string jsonData = JsonConvert.SerializeObject(empObj);
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://titar.ir/api/pc/create_video");
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
                                            if (token["file_name"].ToString() != "")
                                            {
                                                file_name = token["file_name"].ToString();
                                                return file_name;
                                            }

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
                    //    send_content_video( authorid, channelid, title, description, age, language,  tags, price, extension, size, length);
                    //}
                    //else
                    //{
                        return file_name;
                    //}
                }
                return file_name;
            }

    }

      public class SendLike
    {
        public int number_of_try;
        public SendLike()
        {
            number_of_try = 0;
        }
        public class LikeJson
        {
            public string user_id;
            public string content_id;
        }
        public int send_like (string contentid , string userid,string url)
        {
            LikeJson empObj = new LikeJson();
            empObj.content_id = contentid;
            empObj.user_id = userid;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(empObj);
            try
            {
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
                //    send_like(contentid,userid,url);
                //}
                //else
                //{
                    return 1;
               // }
            }
            return 1;
        }
    }
}
