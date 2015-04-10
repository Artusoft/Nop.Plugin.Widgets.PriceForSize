using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Domain
{
  public class AttributeValueEditModel
  {
    public virtual Int32   ProductAttributeValueId { get; set; }
    public virtual String  AttributeName { get; set; }
    public virtual String  ValueName { get; set; }
    public virtual Decimal? PriceForM1 { get; set; }
    public virtual Decimal? PriceForM2 { get; set; }
    public virtual Decimal? PriceForM3 { get; set; }
    public virtual Decimal? PriceForBaseLength { get; set; }
    public virtual Decimal? PriceForHeightLength { get; set; }
    public virtual Decimal? PriceForDepthLength { get; set; }
  }
}
