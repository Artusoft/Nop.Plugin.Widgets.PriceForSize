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

			Property(m => m.StandardPriceType)
				.IsOptional();
			Property(m => m.MeasureDimensionId);

			Property(m => m.WidthAttributeId);
			Property(m => m.MinimumWidthManageable).HasPrecision(18, 4);
      Property(m => m.MaximumWidthManageable).HasPrecision(18, 4);

      Property(m => m.HeightAttributeId);
      Property(m => m.MinimumHeightManageable).HasPrecision(18, 4);
      Property(m => m.MaximumHeightManageable).HasPrecision(18, 4);

      Property(m => m.DepthAttributeId);
      Property(m => m.MinimumDepthManageable).HasPrecision(18, 4);
      Property(m => m.MaximumDepthManageable).HasPrecision(18, 4);

      Property(m => m.MinimumBillablePerimeter);
      Property(m => m.MinimumBillableArea);
      Property(m => m.MinimumBillableVolume);

			this.HasOptional(p => p.MeasureDimension)
						 .WithMany()
						 .HasForeignKey(p => p.MeasureDimensionId);
		}

  }
}
