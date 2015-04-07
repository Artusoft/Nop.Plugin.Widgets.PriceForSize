using Nop.Plugin.Widgets.PriceForSize.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Services
{
  public interface IPriceForSizeService
  {
    void CreateOrUpdatePriceForSize(Product_PriceForSize ps);
    void CreateOrUpdateAttributesPrice(ProductAttributeValue_PriceForSize ps);

    Product_PriceForSize GetPriceForSize(Int32 productId);

    IEnumerable<ProductAttributeValue_PriceForSize> GetAttributesPrice(int productId);
  }

}
