using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace ECourses.Models
{
    public class ImageUpload
    {
      public  ImageUpload()
        {

        }

        public  string PostToImgur(string imagFilePath)
        {

            string imgurApiKey= "7bd067b10fb23cc";
            byte[] imageData;
            int a = 1;
            FileStream fileStream = File.OpenRead(imagFilePath);
            imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);
            fileStream.Close();

            const int MAX_URI_LENGTH = 32766;
            string base64img = System.Convert.ToBase64String(imageData);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < base64img.Length; i += MAX_URI_LENGTH)
            {
                sb.Append(Uri.EscapeDataString(base64img.Substring(i, Math.Min(MAX_URI_LENGTH, base64img.Length - i))));
            }

            string uploadRequestString = "image=" + sb.ToString() + "&key=" + imgurApiKey;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/image");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();




            return "abc";


        }

        public string Upload(string imageAsBase64String)
        {
            string key = "Client-ID 7bd067b10fb23cc";
            XDocument result = null;
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("Authorization", "Client-ID 7bd067b10fb23cc");
            var values = new NameValueCollection
            {
                { "image", imageAsBase64String }
            };
            byte[] response = webClient.UploadValues("http://api.imgur.com/3/image", "POST", values);
            //result = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(response)));
            //    Console.WriteLine("\nResponse received was {0}",Encoding.ASCII.GetString(response));
            var abc = Encoding.ASCII.GetString(response);
            var data = JObject.Parse(abc);
            var abcd = JObject.Parse(data.GetValue("data").ToString());
            string link = abcd.GetValue("link").ToString();


            return link;
        }
    }
}