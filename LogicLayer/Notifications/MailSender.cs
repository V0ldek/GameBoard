using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.Notifications
{

    //TODO change class to static probably
    //TODO remove test function after testing
    //TODO checkout blue note regarding UTF8

    public class MailSender
    {
        private static string Domain = "sandboxab3eb41cb07d4d88b609637cf7d3bd9a.mailgun.org";
        private static string ApiKey = "53fb9863bf954463a14bd144dd943b4e-985b58f4-05d2d853";

        public void Test() // temporary test function
        {
            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName; //temporary
            directory = Path.Combine(directory, "LogicLayer","Notifications", "inlined", "email-confirmation.html");
            SendSimpleMessageHttp(new string[] {
                "miszalek1998@wp.pl", "zirrock2@gmail.com"
            }, new Notification(directory, "Please confirm your email address")).Wait();
        }

        public async Task SendSimpleMessageHttp(string[] recipients, Notification notification)
        {
            var text = File.ReadAllText(notification.HtmlPath);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api" + ":" + ApiKey)));

            var recipientsJoined = String.Join(", ", recipients);

            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                ["from"] = "GameBoard <mailgun@sandboxab3eb41cb07d4d88b609637cf7d3bd9a.mailgun.org>",
                ["to"] = recipientsJoined,
                ["subject"] = notification.Subject,
                ["html"] = text
            };

            var response = await client.PostAsync(
                "https://api.mailgun.net/v2/" + Domain + "/messages",
                new FormUrlEncodedContent(form));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("Success");
            }
            else
            {
                Debug.WriteLine("StatusCode: " + response.StatusCode);
                Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);
            }
        }

    }
}