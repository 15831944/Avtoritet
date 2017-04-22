using System.Web.Http;

namespace PushNotification
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });

            config.Routes.MapHttpRoute("DefaultApiAction", "api/{controller}/{action}/{id}", new
            {
                id = RouteParameter.Optional
            });

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}