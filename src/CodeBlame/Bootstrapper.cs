using JustForFun.NancyAndEventStore.Web.EventStore;
using Nancy;

namespace JustForFun.NancyAndEventStore.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
            var connection = EventStoreConnectionProvider.EventStore;
            connection.Connect();
            container.Register(connection);
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Scripts"));
            nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("Content"));
        }
    }
}