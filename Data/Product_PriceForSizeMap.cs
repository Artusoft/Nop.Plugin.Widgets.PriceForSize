using Nop.Plugin.Widgets.PriceForSize.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Data
{
  public class Product_PriceForSizeMap : EntityTypeConfiguration<Product_PriceForSize>
  {
    public Product_PriceForSizeMap()
    {
      ToTable("Product_PriceForSize");

      //Map the primary key
      HasKey(m => m.Id);

      Property(m => m.ProductId).IsRequired();
      Property(m => m.HasPriceForSize);

      Property(m => m.WidthAttributeId);
      Property(m => m.MinimumWidthManageable);
      Property(m => m.MaximumWidthManageable);

      Property(m => m.HeightAttributeId);
      Property(m => m.MinimumHeightManageable);
      Property(m => m.MaximumHeightManageable);

      Property(m => m.DepthAttributeId);
      Property(m => m.MinimumDepthManageable);
      Property(m => m.MaximumDepthManageable);
    }

  }
}
