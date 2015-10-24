using Nop.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Domain
{
  public class ProductAttributeValue_PriceForSize : BaseEntity
  {
    public virtual Int32 ProductAttributeValueId { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForM1 { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForM2 { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForM3 { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForBaseLength { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForHeightLength { get; set; }
		[UIHint("DecimalNullable")]
		public virtual Decimal? PriceForDepthLength { get; set; }
  }
}
