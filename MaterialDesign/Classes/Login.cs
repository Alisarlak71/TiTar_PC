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
using System.Net;
namespace MaterialDesign2.Classes
{
    public class jsonlogin
    {
        public string email;
        public string password;
       
    }
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string age { get; set; }
        public string profile_image { get; set; }
        public string wallet { get; set; }
        public string birthday { get; set; }
        public string info_confirm { get; set; }
        public string phone_confirm { get; set; }
        public string email_confirm { get; set; }
        public string address_confirm { get; set; }
        public string code_meli { get; set; }
        public string address { get; set; }
        public string id_image { get; set; }
        public string sex { get; set; }
        public string role { get; set; }
        public string grade { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string pushe { get; set; }
        public string firebase { get; set; }
        public string create_date { get; set; }
        public string update_date { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class jsonregister
    {
        public string name;
        public string email;
        public string password;
    }
    public class Login
    {
        public bool status;
        public bool Loged_in;
        public int number_of_try;
        /// <summary>
        /// log in to account
        /// </summary>
        public void Login_To_Account(string email, string password)
        {
            jsonlogin jlogin = new jsonlogin();
            jlogin.email = email;
            jlogin.password = password;
            // Convert Employee object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(jlogin);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://titar.ir/api/pc/login");
                request.Content = new StringContent(jsonData,
                                                    Encoding.UTF8,
                                                    "application/json");
                request.Version = HttpVersion.Version10; 
                try
                {
                using (HttpResponseMessage response = client.SendAsync(request).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent =  content.ReadAsStringAsync().Result;
                        string data = string.Empty;
                        {
                            data = mycontent;
                            if (data!="")
                            {
                            JToken token = JToken.Parse(data);
                            
                            if (token["error"].ToString() == "False")
                            {
                                if (token["user"].ToString() != "")
                                {
                                    User tempuser = new User();
                                    tempuser.id=token["user"]["id"].ToString();
                                    tempuser.name = token["user"]["name"].ToString();
                                    (MaterialDesign.App.Current as MaterialDesign.App).User = tempuser;
                                    status = true;
                                    Loged_in = true;
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
                    //if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true && number_of_try < 6)
                    //{
                    //    number_of_try+=1;
                    //    Login_To_Account(email, password);
                    //}
                    //else
                    //{
                        (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                        return;
                    //}
                }
            }
        }

        /// <summary>
        ///  register in titar
        /// </summary>

        public void Register_In_Titar(string name ,string email, string password)
        {
                jsonregister empObj1 = new jsonregister();
                empObj1.email = email;
                empObj1.password = password;
                empObj1.name = name;
                // Convert Login object to JOSN string format   
                string jsonData = JsonConvert.SerializeObject(empObj1);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://titar.ir/api/pc/register");
                    request.Content = new StringContent(jsonData.ToString(),
                                                        Encoding.UTF8,
                                                        "application/json");
                    request.Version = HttpVersion.Version10;
                    try
                    {
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
                                            if (token["user"].ToString() != "")
                                            {
                                                User tempuser = new User();
                                                tempuser.id = token["user"]["id"].ToString();
                                                tempuser.name = token["user"]["name"].ToString();
                                                (MaterialDesign.App.Current as MaterialDesign.App).User = tempuser;
                                                status = true;
                                                Loged_in = true;
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
                    //if ((MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.IsNetworkAvailable() == true && number_of_try < 6)
                    //{
                    //    number_of_try+=1;
                    //    Register_In_Titar(name, email, password);
                    //}
                    //else
                    //{
                        (MaterialDesign.App.Current as MaterialDesign.App).Internet_connect.Connected = false;
                        return;
                    //}
                }
                }
            }
        

        public Login()
        {
            status = false;
            Loged_in = false;
            number_of_try=0;
        }
        
    }
}
