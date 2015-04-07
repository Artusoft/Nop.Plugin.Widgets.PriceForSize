using Nop.Core;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.PriceForSize.Data;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.PriceForSize
{
  public class PriceForSizePlugin : BasePlugin, IWidgetPlugin, IConsumer<AdminTabStripCreated>
    {
    private readonly PriceForSizeObjectContext _context;
    private readonly IWebHelper _webHelper;

    public PriceForSizePlugin(PriceForSizeObjectContext context, IWebHelper webHelper)
        {
            _context = context;
            this._webHelper = webHelper;
        }

        public override void Install()
        {
            _context.Install();
            base.Install();
        }

        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
          return new List<string> { "productdetails_overview_top" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
          actionName = "Configure";
          controllerName = "WidgetsNivoSlider";
          routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.NivoSlider.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
          actionName = "PublicInfo";
          controllerName = "WidgetsPriceForSize";
          routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.PriceForSize.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public void HandleEvent(AdminTabStripCreated eventMessage)
        {
          if (eventMessage.TabStripName == "product-edit")
          {
            var id = ((Nop.Web.Framework.Mvc.BaseNopEntityModel)eventMessage.Helper.ViewData.Model).Id;

            string url = "/admin/plugins/priceforsize/AdminProduct/" + id.ToString(); //"/ProductKey/GetProductKey?productId=" + productId;
            string tabName = "Price for size" ; //_localizationService.GetResource("Nop.Plugin.Misc.LicenseKey");
            var sb = new StringBuilder();

            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append(Environment.NewLine);
            sb.Append("$(document).ready(function () {");
            sb.Append(Environment.NewLine);
            sb.Append("var kTabs = $('#product-edit').data('kendoTabStrip');");
            sb.Append(Environment.NewLine);
            sb.Append(" kTabs.append({ text: \"" + tabName + "\", contentUrl: \"" + url + "\" });");
            sb.Append(Environment.NewLine);
            sb.Append("});");
            sb.Append(Environment.NewLine);
            sb.Append("</script>");
            sb.Append(Environment.NewLine);
            eventMessage.BlocksToRender.Add(MvcHtmlString.Create(sb.ToString()));

          }

        }

    }
}
