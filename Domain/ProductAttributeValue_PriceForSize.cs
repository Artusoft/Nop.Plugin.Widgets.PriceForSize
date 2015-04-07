using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Domain
{
  public class ProductAttributeValue_PriceForSize : BaseEntity
  {
    public virtual Int32 ProductAttributeValueId { get; set; }
    public virtual Decimal? PriceForM1 { get; set; }
    public virtual Decimal? PriceForM2 { get; set; }
    public virtual Decimal? PriceForM3 { get; set; }
  }
}
