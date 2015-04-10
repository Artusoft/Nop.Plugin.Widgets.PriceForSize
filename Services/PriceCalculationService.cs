using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Services
{
  public class PriceCalculationService : Nop.Services.Catalog.PriceCalculationService
  {
    IProductAttributeParser _productAttributeParser;
    IProductService _productService;
    IPriceForSizeService _priceForSizeService;

    public PriceCalculationService(IWorkContext workContext,
            IStoreContext storeContext,
            IDiscountService discountService,
            ICategoryService categoryService,
            IProductAttributeParser productAttributeParser,
            IProductService productService,
            ICacheManager cacheManager,
            ShoppingCartSettings shoppingCartSettings,
            CatalogSettings catalogSettings,
            IPriceForSizeService priceForSizeService)
      : base(
      workContext,
      storeContext,
      discountService,
      categoryService,
      productAttributeParser,
      productService,
      cacheManager,
      shoppingCartSettings,
      catalogSettings
      )
    {
      _productAttributeParser = productAttributeParser;
      _productService = productService;
      _priceForSizeService = priceForSizeService;
    }

    public override decimal GetProductCost(Product product, string attributesXml)
    {
      return base.GetProductCost(product, attributesXml);
    }

    public override decimal GetUnitPrice(Product product, Core.Domain.Customers.Customer customer, ShoppingCartType shoppingCartType, int quantity, string attributesXml, decimal customerEnteredPrice, DateTime? rentalStartDate, DateTime? rentalEndDate, bool includeDiscounts, out decimal discountAmount, out Core.Domain.Discounts.Discount appliedDiscount)
    {
      var ps = _priceForSizeService.GetPriceForSize(product.Id);

      if (ps != null && ps.HasPriceForSize)
      {
        Int32 w = 0;
        Int32 h = 0;
        Int32 d = 0;

        var mappings = _productAttributeParser.ParseProductAttributeMappings(attributesXml);

        var bAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.WidthAttributeId);
        if (bAttr != null)
          w = _productAttributeParser.ParseValues(attributesXml, bAttr.Id).Select(s => Convert.ToInt32(s)).FirstOrDefault();

        var hAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.HeightAttributeId);
        if (hAttr != null)
          h = _productAttributeParser.ParseValues(attributesXml, hAttr.Id).Select(s => Convert.ToInt32(s)).FirstOrDefault();

        var dAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.DepthAttributeId);
        if (dAttr != null)
          d = _productAttributeParser.ParseValues(attributesXml, hAttr.Id).Select(s => Convert.ToInt32(s)).FirstOrDefault();

        Decimal priceM1 = 0;
        Decimal priceM2 = 0;
        Decimal priceM3 = 0;
        Decimal priceBase = 0;
        Decimal priceHeight = 0;
        Decimal priceDepth = 0;

        var prices = _priceForSizeService.GetAttributesPrice(product.Id);
        foreach (var m in mappings)
          if (m.ProductAttributeId != ps.WidthAttributeId && m.ProductAttributeId != ps.HeightAttributeId)
          {
            var values = _productAttributeParser.ParseValues(attributesXml, m.Id);
            foreach (var v in values)
            {
              Int32 id = 0;
              if (Int32.TryParse(v, out id))
              {
                var price = (from p in prices
                             where p.ProductAttributeValueId == id
                             select p).FirstOrDefault();
                if (price != null)
                {
                  priceM1 += price.PriceForM1.GetValueOrDefault();
                  priceM2 += price.PriceForM2.GetValueOrDefault();
                  priceM3 += price.PriceForM3.GetValueOrDefault();
                  priceBase += price.PriceForBaseLength.GetValueOrDefault();
                  priceHeight += price.PriceForHeightLength.GetValueOrDefault();
                  priceDepth += price.PriceForDepthLength.GetValueOrDefault();
                }
              }
            }
          }

        var per = Convert.ToDecimal(w + h) * 2 / 100;
        var area = Convert.ToDecimal(w) / 100 * Convert.ToDecimal(h) / 100;
        var vol = Convert.ToDecimal(w) / 100 * Convert.ToDecimal(h) / 100 * Convert.ToDecimal(d) / 100;

        if (ps.MinimumBillablePerimeter.HasValue && per < ps.MinimumBillablePerimeter.Value)
          per = ps.MinimumBillablePerimeter.Value;

        if (ps.MinimumBillableArea.HasValue && area < ps.MinimumBillableArea.Value)
          area = ps.MinimumBillableArea.Value;

        if (ps.MinimumBillableVolume.HasValue && vol < ps.MinimumBillableVolume.Value)
          vol = ps.MinimumBillableVolume.Value;

        return base.GetUnitPrice(product, customer, shoppingCartType, quantity, attributesXml, customerEnteredPrice, rentalStartDate, rentalEndDate, includeDiscounts, out  discountAmount, out appliedDiscount) +
          per * priceM1 +
          area * priceM2 +
          vol * priceM3 +
          priceBase * w +
          priceHeight * h +
          priceDepth * d;
      }
      else
        return base.GetUnitPrice(product, customer, shoppingCartType, quantity, attributesXml, customerEnteredPrice, rentalStartDate, rentalEndDate, includeDiscounts, out discountAmount, out appliedDiscount);
    }


  }
}
