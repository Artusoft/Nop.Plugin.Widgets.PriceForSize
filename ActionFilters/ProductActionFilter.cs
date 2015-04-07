using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.PriceForSize.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.PriceForSize.ActionFilters
{
  class ProductActionFilter: ActionFilterAttribute, IFilterProvider
{
    public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
    {
      if (controllerContext.Controller.GetType().FullName == "Nop.Web.Controllers.ProductController" &&
        actionDescriptor.ActionName.Equals("ProductDetails", StringComparison.InvariantCultureIgnoreCase))
        {
            return new List<Filter>() { new Filter(this, FilterScope.Action, 0) };
        }
        return new List<Filter>();
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      base.OnResultExecuted(filterContext);
      if (filterContext.RouteData.Values.ContainsKey("productId"))
      {
        var _priceForSizeService = EngineContext.Current.Resolve<Services.IPriceForSizeService>();
        var ps = _priceForSizeService.GetPriceForSize((Int32)filterContext.RouteData.Values["productId"]);
        if (ps.HasPriceForSize)
        {
          String s = "<script>$().ready(function () {  $('.attributes input[class=\"textbox\"]').change(function(){" +
            String.Format("adjustPrice_{0}();", ps.ProductId) + "});});</script>";

          filterContext.HttpContext.Response.Output.WriteLine(s);
        }
      }
    }
  }
}
