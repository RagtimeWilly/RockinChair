using System;
using System.Configuration;
using Autofac;
using Microsoft.Owin.Hosting;
using RockinChair.SampleApp.Infrastructure.IoC;
using System.Threading.Tasks;

namespace RockinChair.SampleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Bootstrapper.Init();

            var port = ConfigurationManager.AppSettings["Port"];

            var options = new StartOptions($"http://*:{port}")
            {
                ServerFactory = "Microsoft.Owin.Host.HttpListener"
            };

            using (var scope = Bootstrapper.Container.BeginLifetimeScope())
            {
                var startup = scope.Resolve<Startup>();

                var service = scope.Resolve<Service>();

                Task.Run(() => service.Start());

                // Start OWIN host 
                using (WebApp.Start(options, startup.Configuration))
                {
                    foreach (var url in options.Urls)
                    {
                        Console.WriteLine($"Service listening on {url}");
                    }

                    Console.ReadLine();
                }
            }
        }
    }
}
