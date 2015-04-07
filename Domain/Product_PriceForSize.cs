using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.PriceForSize.Domain
{
  public class Product_PriceForSize : BaseEntity
  {
    public virtual Int32 ProductId { get; set; }
    public virtual bool HasPriceForSize { get; set; }

    public virtual Int32? WidthAttributeId { get; set; }
    public virtual Int32? MinimumWidthManageable { get; set; }
    public virtual Int32? MaximumWidthManageable { get; set; }

    public virtual Int32? HeightAttributeId { get; set; }
    public virtual Int32? MinimumHeightManageable { get; set; }
    public virtual Int32? MaximumHeightManageable { get; set; }

    public virtual Int32? DepthAttributeId { get; set; }
    public virtual Int32? MinimumDepthManageable { get; set; }
    public virtual Int32? MaximumDepthManageable { get; set; }
  }
}
