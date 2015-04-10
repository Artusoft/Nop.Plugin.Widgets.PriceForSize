using Nop.Core.Data;
using Nop.Plugin.Widgets.PriceForSize.Data;
using Nop.Plugin.Widgets.PriceForSize.Domain;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Services
{
  public class PriceForSizeService : IPriceForSizeService
  {
    private readonly IRepository<Product_PriceForSize> _productRepository;
    private readonly IRepository<ProductAttributeValue_PriceForSize> _productAttributeValueRepository_ps;
    private readonly IProductAttributeService _productAttributeService;

    public PriceForSizeService(IRepository<Product_PriceForSize> productRepository, 
      IRepository<ProductAttributeValue_PriceForSize> productAttributeValueRepository_ps,
      IProductAttributeService productAttributeService)
    {
      _productRepository = productRepository;
      _productAttributeValueRepository_ps = productAttributeValueRepository_ps;
      _productAttributeService = productAttributeService;
    }

    public void CreateOrUpdatePriceForSize(Product_PriceForSize ps)
    {
      var p = _productRepository.Table.SingleOrDefault(_p => _p.ProductId == ps.ProductId);
      if (p != null)
      {
        p.HasPriceForSize = ps.HasPriceForSize;

        p.WidthAttributeId = ps.WidthAttributeId;
        p.MinimumWidthManageable = ps.MinimumWidthManageable;
        p.MaximumWidthManageable = ps.MaximumWidthManageable;

        p.HeightAttributeId = ps.HeightAttributeId;
        p.MinimumHeightManageable = ps.MinimumHeightManageable;
        p.MaximumHeightManageable = ps.MaximumHeightManageable;

        p.DepthAttributeId = ps.DepthAttributeId;
        p.MinimumDepthManageable = ps.MinimumDepthManageable;
        p.MaximumDepthManageable = ps.MaximumDepthManageable;

        p.MinimumBillablePerimeter = ps.MinimumBillablePerimeter;
        p.MinimumBillableArea = ps.MinimumBillableArea;
        p.MinimumBillableVolume = ps.MinimumBillableVolume;

        _productRepository.Update(p);
      }
      else
        _productRepository.Insert(ps);
    }

    public void CreateOrUpdateAttributesPrice(ProductAttributeValue_PriceForSize ps)
    {
      var p = _productAttributeValueRepository_ps.Table.SingleOrDefault(_p => _p.ProductAttributeValueId == ps.ProductAttributeValueId);
      if (p != null)
      {
        p.PriceForM1 = ps.PriceForM1;
        p.PriceForM2 = ps.PriceForM2;
        p.PriceForM3 = ps.PriceForM3;
        p.PriceForBaseLength = ps.PriceForBaseLength;
        p.PriceForHeightLength = ps.PriceForHeightLength;
        p.PriceForDepthLength = ps.PriceForDepthLength;

        _productAttributeValueRepository_ps.Update(p);
      }
      else
        _productAttributeValueRepository_ps.Insert(ps);
    }

    public Product_PriceForSize GetPriceForSize(int productId)
    {
      var retVal = (from p in _productRepository.Table
                    where p.ProductId == productId
                    select p).SingleOrDefault();
      if (retVal == null)
        retVal = new Product_PriceForSize()
        {
          ProductId = productId
        };
      return retVal;
    }

    public IEnumerable<ProductAttributeValue_PriceForSize> GetAttributesPrice(int productId)
    {
      var m = from _m in _productAttributeService.GetProductAttributeMappingsByProductId(productId)
              from _v in _m.ProductAttributeValues
              select _v.Id;

      return (from p in _productAttributeValueRepository_ps.Table
             where m.Contains(p.ProductAttributeValueId)
             select p).ToList(); 
    }



  }

}
