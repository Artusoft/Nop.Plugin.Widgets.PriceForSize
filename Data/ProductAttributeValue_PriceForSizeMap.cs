using Nop.Plugin.Widgets.PriceForSize.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Data
{
  public class ProductAttributeValue_PriceForSizeMap : EntityTypeConfiguration<ProductAttributeValue_PriceForSize>
  {
    public ProductAttributeValue_PriceForSizeMap()
    {
      ToTable("ProductAttributeValue_PriceForSize");

      //Map the primary key
      HasKey(m => m.Id);

      Property(m => m.ProductAttributeValueId).IsRequired();
      Property(m => m.PriceForM1);
      Property(m => m.PriceForM2);
      Property(m => m.PriceForM3);

      Property(m => m.PriceForBaseLength);
      Property(m => m.PriceForHeightLength);
      Property(m => m.PriceForDepthLength);
    }

  }
}
