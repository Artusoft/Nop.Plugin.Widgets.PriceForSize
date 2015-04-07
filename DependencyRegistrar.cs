using Autofac;
using Autofac.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.PriceForSize.Data;
using Nop.Plugin.Widgets.PriceForSize.Domain;
using Nop.Plugin.Widgets.PriceForSize.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc;
using Nop.Services.Catalog;

namespace Nop.Plugin.Widgets.PriceForSize
{
  public class DependencyRegistrar : IDependencyRegistrar

  {
    private const string CONTEXT_NAME = "nop_object_context_product_price_for_size";

    public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
    {
      builder.RegisterType<PriceForSizeService>().As<IPriceForSizeService>().InstancePerLifetimeScope();

      //data context
      this.RegisterPluginDataContext<PriceForSizeObjectContext>(builder, CONTEXT_NAME);

      //override required repository with our custom context
      builder.RegisterType<EfRepository<Product_PriceForSize>>()
          .As<IRepository<Product_PriceForSize>>()
          .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
          .InstancePerLifetimeScope();

      builder.RegisterType<EfRepository<ProductAttributeValue_PriceForSize>>()
          .As<IRepository<ProductAttributeValue_PriceForSize>>()
          .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
          .InstancePerLifetimeScope();

      builder.RegisterType<Nop.Plugin.Widgets.PriceForSize.Services.PriceCalculationService>().As<IPriceCalculationService>().InstancePerLifetimeScope();

     // builder.RegisterType<Nop.Plugin.Widgets.PriceForSize.ActionFilters.ProductActionFilter>().As<System.Web.Mvc.IFilterProvider>();
    }

    public int Order
    {
      get { return 1; }
    }

  }
}
