using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EmailSubscriptionScheduler
{
    #region
    public enum ActionStatus
    {
        Successfull = 200,
        Error = 400,
        LoggedOut = 201,
        Unauthorized = 202
    }
    public class ActionOutputBase
    {
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
    }
    public class ActionOutput : ActionOutputBase
    {
        public long ID { get; set; }
    }
    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // TODO - Send HTTP requests                
                //client.BaseAddress = new Uri("http://localhost:50172/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("LicenseKey", "594ccbc243ea0efbf07df44c966d0d32");
                client.DefaultRequestHeaders.Add("ClientHash", "838e8a75e9b9ceb1ee12592ed4eea8f51c609921");
                client.DefaultRequestHeaders.Add("TimeStamp", "12345678");
                client.DefaultRequestHeaders.Add("DeviceToken", "NoToken-APi-Hit");
                client.DefaultRequestHeaders.Add("DeviceType", "7");

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/home/SendSubscriptionJokes");
                if (response.IsSuccessStatusCode)
                {
                    ActionOutput output = await response.Content.ReadAsAsync<ActionOutput>();
                    Console.WriteLine("{0}", output.Message);
                }
            }
        }
    }
}
