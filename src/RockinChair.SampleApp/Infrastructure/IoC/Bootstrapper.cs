using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Net.Http;

namespace RockinChair.SampleApp.Infrastructure.IoC
{
    internal class Bootstrapper
    {
        private static IContainer _container = null;

        public static IContainer Container
        {
            get
            {
                if (_container == null)
                    throw new Exception("Container has not been initialized");

                return _container;
            }
        }

        public static void Init()
        {
            if (_container != null)
                return;

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Func<HttpClient> httpClientFunc = () => new HttpClient();

            builder.Register<Func<string, SpotifyAuthorizedClient>>(
                c => (s) => new SpotifyAuthorizedClient(httpClientFunc, s)).AsSelf();

            builder.RegisterType<SpotifyAuthorizer>().WithParameter("clientFactory", httpClientFunc).AsSelf();

            builder.RegisterType<Service>().AsSelf().SingleInstance();

            builder.RegisterType<Startup>().AsSelf();

            _container = builder.Build();
        }
    }
}
