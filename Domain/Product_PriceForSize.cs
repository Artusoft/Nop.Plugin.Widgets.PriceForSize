using Nop.Core;
using Nop.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Domain
{
	public enum TypeOfPrice 
	{
		Perimeter = 1,
		Area = 2,
		Volume = 3
	}

	public class Product_PriceForSize : BaseEntity
  {
    public virtual Int32 ProductId { get; set; }
    public virtual bool HasPriceForSize { get; set; }

		public virtual TypeOfPrice? StandardPriceType { get; set; }

		/// <summary>
		/// Get or set the size attribute measure
		/// </summary>
		public virtual Int32? MeasureDimensionId { get; set; }

		public virtual Int32? WidthAttributeId { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumWidthManageable { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MaximumWidthManageable { get; set; }

    public virtual Int32? HeightAttributeId { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumHeightManageable { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MaximumHeightManageable { get; set; }

    public virtual Int32? DepthAttributeId { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumDepthManageable { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MaximumDepthManageable { get; set; }

		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumBillablePerimeter { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumBillableArea { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? MinimumBillableVolume { get; set; }

		public virtual MeasureDimension MeasureDimension { get; set; }
	}
}
