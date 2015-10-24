using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Widgets.PriceForSize.Data;
using Nop.Plugin.Widgets.PriceForSize.Domain;
using Nop.Plugin.Widgets.PriceForSize.Services;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.PriceForSize.Controllers
{
  public class WidgetsPriceForSizeController : BasePluginController
  {
    private readonly IStoreContext _storeContext;
    private readonly IPriceForSizeService _priceForSizeService;
    private readonly IProductAttributeService _productAttributeService;
		private readonly IMeasureService _measureService;

		public WidgetsPriceForSizeController(
			IStoreContext storeContext,
			IPriceForSizeService priceForSizeService,
			IProductAttributeService productAttributeService,
			IMeasureService measureService)
		{
			_storeContext = storeContext;
			_priceForSizeService = priceForSizeService;
			_productAttributeService = productAttributeService;
			_measureService = measureService;
		}

    public new RedirectToRouteResult RedirectToAction(string action, RouteValueDictionary routeValues)
    {
      return base.RedirectToAction(action, routeValues);
    }

    [ChildActionOnly]
    public ActionResult PublicInfo(string widgetZone, object additionalData = null)
    {
      dynamic model = ControllerContext.ParentActionViewContext.ParentActionViewContext.ViewData.Model; 
      if (model != null)
      {
        var ps = _priceForSizeService.GetPriceForSize(model.Id);

        return View("~/Plugins/Widgets.PriceForSize/Views/WidgetsPriceForSize/PublicInfo.cshtml", ps);
      }
      else
        return new EmptyResult();
    }

    //public ActionResult Test(Int32 id)
    //{
    //    var ps = _priceForSizeService.GetPriceForSize(id);

    //    return View("~/Plugins/Widgets.PriceForSize/Views/WidgetsPriceForSize/PublicInfo.cshtml", ps);
    //}

    public ActionResult AdminProduct(Int32 id)
    {
      var ps = _priceForSizeService.GetPriceForSize(id);

      var mappings = from m in  _productAttributeService.GetProductAttributeMappingsByProductId(id)
                     select m.ProductAttribute;

			//var items = from v in Enum.GetValues(typeof(TypeOfPrice)).OfType<TypeOfPrice>()
			//						select new
			//						{
			//							Id = (Int16)v,
			//							Name = Enum.GetName(typeof(TypeOfPrice), v)
			//						};

			var dimensions = _measureService.GetAllMeasureDimensions();

			ViewData["attributes"] = new SelectList(mappings,"Id", "Name");
		//	ViewData["standardPriceType"] = new SelectList(items, "Id", "Name", (Int16)ps.StandardPriceType);
			ViewData["measureDimension"] = new SelectList(dimensions, "Id", "Name");

			return View("~/Plugins/Widgets.PriceForSize/Views/WidgetsPriceForSize/AdminProduct.cshtml", ps);
    }

    [HttpPost]
    public ActionResult AdminProduct(Product_PriceForSize product)
    {
      _priceForSizeService.CreateOrUpdatePriceForSize(product);

      return new ContentResult() { Content = "Save completed.", ContentType = "text/html" };
    }

    public ActionResult AdminProductAttributeValues(Int32 id)
    {
      var mappings = from m in _productAttributeService.GetProductAttributeMappingsByProductId(id)
                     select m;

      var prices = _priceForSizeService.GetAttributesPrice(id);

      var model = from m in mappings
                  from v in m.ProductAttributeValues
                  select new AttributeValueEditModel()
                  {
                    ProductAttributeValueId = v.Id,
                    AttributeName = m.ProductAttribute.Name,
                    ValueName = v.Name,
                    PriceForM1 = (from _p in prices
                                  where _p.ProductAttributeValueId == v.Id
                                  select _p.PriceForM1).FirstOrDefault(),
                    PriceForM2 = (from _p in prices
                                  where _p.ProductAttributeValueId == v.Id
                                  select _p.PriceForM2).FirstOrDefault(),
                    PriceForM3 = (from _p in prices
                                  where _p.ProductAttributeValueId == v.Id
                                  select _p.PriceForM3).FirstOrDefault(),
                    PriceForBaseLength = (from _p in prices
                                          where _p.ProductAttributeValueId == v.Id
                                          select _p.PriceForBaseLength).FirstOrDefault(),
                    PriceForHeightLength = (from _p in prices
                                            where _p.ProductAttributeValueId == v.Id
                                            select _p.PriceForHeightLength).FirstOrDefault(),
                    PriceForDepthLength = (from _p in prices
                                           where _p.ProductAttributeValueId == v.Id
                                           select _p.PriceForDepthLength).FirstOrDefault()
                  };


      return View("~/Plugins/Widgets.PriceForSize/Views/WidgetsPriceForSize/AdminProductAttributeValues.cshtml", model.ToList());
    }

    [HttpPost]
    public ActionResult AdminProductAttributeValues(List<AttributeValueEditModel> prices)
    {
      foreach (var p in prices)
        _priceForSizeService.CreateOrUpdateAttributesPrice(new ProductAttributeValue_PriceForSize()
        {
          ProductAttributeValueId = p.ProductAttributeValueId,
          PriceForM1 = p.PriceForM1,
          PriceForM2 = p.PriceForM2,
          PriceForM3 = p.PriceForM3,
          PriceForBaseLength = p.PriceForBaseLength,
          PriceForHeightLength = p.PriceForHeightLength,
          PriceForDepthLength = p.PriceForDepthLength
        });

      return new ContentResult() { Content = "Save completed.", ContentType = "text/html" };
    }

  }
}
