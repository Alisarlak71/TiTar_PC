using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesign2.Classes
{
    public class Banners
    {
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string link { get; set; }

        public List<Banners> get_banners()
        {
            List<Banners> classdata = new List<Banners>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                    Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                    string url = variables["BaseUrl"] + "getBanners";

                    HttpResponseMessage response = client.GetAsync(url).Result;

                    HttpContent content = response.Content;

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
                            if (token["banners"].ToString() != "")
                            {
                                int count = token["banners"].Count();
                                int i = 0;
                                while (i < count)
                                {
                                    Banners temp = new Banners();
                                    temp.link = token["banners"][i]["link"].ToString();
                                    temp.thumbnail = token["banners"][i]["thumbnail"].ToString();
                                    temp.title = token["banners"][i]["title"].ToString();
                                    classdata.Add(temp);
                                    i++;
                                }
                            }
                        }
                    }
                    return classdata;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
   