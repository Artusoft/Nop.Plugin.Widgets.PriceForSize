using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.PriceForSize
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
           var routek = routes.MapRoute("Nop.Plugin.Widgets.PriceForSize",
               "admin/plugins/priceforsize/{action}/{Id}",
               new { controller = "WidgetsPriceForSize", action = "AdminProduct", Id = UrlParameter.Optional },
              // new { Id = @"\d+" },
               new[] { "Nop.Plugin.Widgets.PriceForSize.Controllers" }
            );

            routek.DataTokens.Add("area", "admin");
            routes.Remove(routek);
            routes.Insert(0, routek);
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
