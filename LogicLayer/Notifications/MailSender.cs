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
        private static string Domain = "";
        private static string ApiKey = "";

        public void Test() // temporary test function
        {
            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName; //temporary
            directory = Path.Combine(directory, "LogicLayer","Notifications", "inlined", "email-confirmation.html");
            SendSimpleMessageHttp(new Recipient("zirrock2@gmail.com", "https://wp.pl"), 
                new Notification(directory, "Please confirm your email address")).Wait();
        }

        public async Task SendSimpleMessageHttp(Recipient recipient, Notification notification)
        {
            var text = File.ReadAllText(notification.HtmlPath);

            try
            {
                text = text.Replace("#@redirectLink@#", recipient.NotificationLink);
            }
            catch
            {
                Debug.Write("Link is null");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api" + ":" + ApiKey)));

            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                ["from"] = "GameBoard <mailgun@sandboxab3eb41cb07d4d88b609637cf7d3bd9a.mailgun.org>",
                ["to"] = recipient.EmailAddress,
                ["subject"] = notification.Subject,
                ["html"] = text
            };

            var response = await client.PostAsync(
                "https://api.mailgun.net/v3/" + Domain + "/messages",
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