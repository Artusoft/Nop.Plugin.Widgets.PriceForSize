using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Directory;
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
		private readonly IProductAttributeParser _productAttributeParser;
		private readonly IProductService _productService;
		private readonly IPriceForSizeService _priceForSizeService;
		private readonly IMeasureService _measureService;

		public PriceCalculationService(IWorkContext workContext,
            IStoreContext storeContext,
            IDiscountService discountService,
						IManufacturerService manufacturerService,
						ICategoryService categoryService,
            IProductAttributeParser productAttributeParser,
            IProductService productService,
            ICacheManager cacheManager,
            ShoppingCartSettings shoppingCartSettings,
            CatalogSettings catalogSettings,
            IPriceForSizeService priceForSizeService,
						IMeasureService measureService)
      : base(
      workContext,
      storeContext,
      discountService,
			categoryService,
			manufacturerService,
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
			_measureService = measureService;
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
        Decimal w = 0;
				Decimal h = 0;
				Decimal d = 0;

        var mappings = _productAttributeParser.ParseProductAttributeMappings(attributesXml);


        var bAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.WidthAttributeId);
				if (bAttr != null)
					w = _productAttributeParser.ParseValues(attributesXml, bAttr.Id).Select(s => Convert.ToDecimal(s)).FirstOrDefault();

        var hAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.HeightAttributeId);
        if (hAttr != null)
          h = _productAttributeParser.ParseValues(attributesXml, hAttr.Id).Select(s => Convert.ToDecimal(s)).FirstOrDefault();

        var dAttr = mappings.SingleOrDefault(m => m.ProductAttributeId == ps.DepthAttributeId);
        if (dAttr != null)
          d = _productAttributeParser.ParseValues(attributesXml, hAttr.Id).Select(s => Convert.ToDecimal(s)).FirstOrDefault();

				if (ps.MeasureDimension != null)
				{
					w = _measureService.ConvertToPrimaryMeasureDimension(w, ps.MeasureDimension);
					h = _measureService.ConvertToPrimaryMeasureDimension(h, ps.MeasureDimension);
					d = _measureService.ConvertToPrimaryMeasureDimension(d, ps.MeasureDimension);
				}

				if (ps.MinimumWidthManageable.HasValue && w < ps.MinimumWidthManageable)
					w = ps.MinimumWidthManageable.Value;
				if (ps.MaximumWidthManageable.HasValue && w > ps.MaximumWidthManageable)
					w = ps.MaximumWidthManageable.Value;

				if (ps.MinimumHeightManageable.HasValue && h < ps.MinimumHeightManageable)
					h = ps.MinimumHeightManageable.Value;
				if (ps.MaximumHeightManageable.HasValue && h > ps.MaximumHeightManageable)
					h = ps.MaximumHeightManageable.Value;

				if (ps.MinimumDepthManageable.HasValue && d < ps.MinimumDepthManageable)
					d = ps.MinimumDepthManageable.Value;
				if (ps.MaximumDepthManageable.HasValue && d > ps.MaximumDepthManageable)
					d = ps.MaximumDepthManageable.Value;


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

        var per = (w + h) * 2;
        var area = w * h;
				var vol = w * h * d;

        if (ps.MinimumBillablePerimeter.HasValue && per < ps.MinimumBillablePerimeter.Value)
          per = ps.MinimumBillablePerimeter.Value;

        if (ps.MinimumBillableArea.HasValue && area < ps.MinimumBillableArea.Value)
          area = ps.MinimumBillableArea.Value;

        if (ps.MinimumBillableVolume.HasValue && vol < ps.MinimumBillableVolume.Value)
          vol = ps.MinimumBillableVolume.Value;

				var standardPrice = base.GetUnitPrice(product, customer, shoppingCartType, quantity, attributesXml, customerEnteredPrice, rentalStartDate, rentalEndDate, includeDiscounts, out discountAmount, out appliedDiscount);

				switch (ps.StandardPriceType)
				{
					case Domain.TypeOfPrice.Perimeter:
						standardPrice = standardPrice * per;
            break;

					case Domain.TypeOfPrice.Area:
						standardPrice = standardPrice * area;
						break;

					case Domain.TypeOfPrice.Volume:
						standardPrice = standardPrice * vol;
						break;
				}


				return standardPrice +
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
