using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Mail;

namespace ECourses.Models
{
    public class ImageUpload
    {
      public  ImageUpload()
        {

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

        public void sendFCMtoTeacher( string device, string course, string name)
        {

          string sender = "28924678047";
            // device = "cKzHju-6K1g:APA91bEvRWVqTQX2PjXiZQ6gv-niTrc9ye3JoHVgM6cm4f1PLZrWOL9hhZqFMm9taMbTm4S_UM71WKSIXMObFhNd4g4pmE2pWALZML45jbl2wH8jKTdEnTQNk--PIrWxh2iljlSD-TSb";
            //device = "eX_fFCHGOb0:APA91bFSsf5v1PQLFJ6bfnmKXaSMoUEO94Py8jkAuscjK4h0Fyc9ezFunA2gWl6v3WuybaYfdhDNbfv9JHw4KLvpRs1O9_DK7qw4o2Y6d0KuENr9RJ1H08oWSI0AbjgBu0JOQkBRabf7";
            string message = name + " Subscribe your course " + course;
            try
            {
                var applicationID = "AAAABrwLj58:APA91bHSGZZ-QEidKCGgeRmd0ippqd90oJyuME2kBeRY6QUtJn4__OXUDRZ-kkB9TCLcOuJAxljGB13oGKL1PHOBesdqo19gy-FVwaL8h0-5k2xXBybww4yMYF8QkacYfSF5Y4rOK5HL";
                var senderId = sender;
                string deviceId = device;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data1 = new
                {
                    to = deviceId,
                    data = new
                    {
                        body = message,
                        title = "Subscription Course",
                        icon = "myicon"
                    },
                    priority = "high"

                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data1);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //  Response.Write(sResponseFromServer);
                                // Label3.Text = sResponseFromServer;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Response.Write(ex.Message);
            }


        }




        public void sendFCMtoStudent(string device, string course, string name)
        {

            // string sender = "28924678047";
            string sender = "239318098717";
           // device = "cB6wlFXj3Wk:APA91bH8oyFS1rq_5qMzzohzCB0JP2-dVWsxYzw8gbYBTPOREmrcNxYsvrfZmf3z0ZAWTBaufs5_bz-X8MVQwJonbf8CtstDuF4rCdYzXZJAS7Mu4HujIWtB6lLu0zWqxF4l1xGNslM1";

            string message = name + " Update " + course;
            try
            {
                //  var applicationID = "AAAABrwLj58:APA91bHSGZZ-QEidKCGgeRmd0ippqd90oJyuME2kBeRY6QUtJn4__OXUDRZ-kkB9TCLcOuJAxljGB13oGKL1PHOBesdqo19gy-FVwaL8h0-5k2xXBybww4yMYF8QkacYfSF5Y4rOK5HL";
                var applicationID = "AAAAN7h4Yx0:APA91bEUSEK7u8HPprAfb64ZRqPoksLy3MbVDGYxngGQmcmaGZPspkw2I2Eo23gMlnKkTXJ-EqR2gjjEgF1KjeyiH1ztA5g1TUgMUwQ9YRiBMfmdJEt2pxZv0OKXi-sm74lghG2LocE7";

                var senderId = sender;
                string deviceId = device;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data1 = new
                {
                    to = deviceId,
                    data = new
                    {
                        body = message,
                        title = "Updated Course",
                        icon = "myicon"
                    },
                    priority = "high"

                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data1);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //  Response.Write(sResponseFromServer);
                                // Label3.Text = sResponseFromServer;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Response.Write(ex.Message);
            }


        }

        public void sendEmail(string email,string name,string pass)
        {
            var fromAddress = new MailAddress("dawratty@gmail.com", "Dawratty Help");
            var toAddress = new MailAddress(email, name);
            const string fromPassword = "d1d2d3d4d5d6d7";
            const string subject = "Forget Password";
             string body = "Dear " +name+"! your password is "+ pass+'\n'+'\n'+ "Regards "+'\n'+"Dawratty Team";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }


    }
}