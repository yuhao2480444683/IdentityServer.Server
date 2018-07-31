using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Server {
    public class Program {
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseKestrel(
                options => {
                    options.Listen(IPAddress.Any, 5080);
                    options.Listen(IPAddress.Any, 5090,
                        listenOptions =>
                            listenOptions.UseHttps("iis.pfx", "iis"));
                }).Build();
    }
}