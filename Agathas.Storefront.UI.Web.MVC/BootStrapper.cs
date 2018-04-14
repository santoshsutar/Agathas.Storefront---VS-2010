using Agathas.Storefront.Controllers.ActionArguments;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Infrastructure.Domain.Events;
using Agathas.Storefront.Infrastructure.Logging;
using Agathas.Storefront.Infrastructure.Payments;
using Agathas.Storefront.Infrastructure.UnitOfWork;
using Agathas.Storefront.Infrastructure.Configuration;
using Agathas.Storefront.Model.Basket;
using Agathas.Storefront.Model.Categories;
using Agathas.Storefront.Model.Customers;
using Agathas.Storefront.Model.Orders;
using Agathas.Storefront.Model.Orders.Events;
using Agathas.Storefront.Model.Products;
using Agathas.Storefront.Model.Shipping;
using Agathas.Storefront.Services.DomainEventHandlers;
using Agathas.Storefront.Services.Implementations;
using Agathas.Storefront.Services.Interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;
using Agathas.Storefront.Infrastructure.Email;
using Ninject;

namespace Agathas.Storefront.UI.Web.MVC
{
    public class BootStrapper
    {
        public static void ConfigureDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ControllerRegistry>();

            });
        }
        public static void config(IKernel kernel)
        {
            //kernel.Bind<IValueCalculator>().To<LinqValueCalculator>();
            // Repositories 
            kernel.Bind<IOrderRepository>().To<Repository.NHibernate.Repositories.OrderRepository>();
            kernel.Bind<ICustomerRepository>().To
                     <Repository.NHibernate.Repositories.CustomerRepository>();
            kernel.Bind<IBasketRepository>().To
                     <Repository.NHibernate.Repositories.BasketRepository>();
            kernel.Bind<IDeliveryOptionRepository>().To
                      <Repository.NHibernate.Repositories.DeliveryOptionRepository>();

            kernel.Bind<ICategoryRepository>().To
                     <Repository.NHibernate.Repositories.CategoryRepository>();
            kernel.Bind<IProductTitleRepository>().To
                     <Repository.NHibernate.Repositories.ProductTitleRepository>();
            kernel.Bind<IProductRepository>().To
                     <Repository.NHibernate.Repositories.ProductRepository>();
            kernel.Bind<IUnitOfWork>().To
                     <Repository.NHibernate.NHUnitOfWork>();

            // Order Service
            kernel.Bind<IOrderService>().To
                    <OrderService>();

            // Payment
            kernel.Bind<IPaymentService>().To
                    <PayPalPaymentService>();

            // Handlers for Domain Events
            kernel.Bind<IDomainEventHandlerFactory>().To<StructureMapDomainEventHandlerFactory>();
            kernel.Bind<IDomainEventHandler<OrderSubmittedEvent>>()
                   .To<OrderSubmittedHandler>();


            // Product Catalogue                                         
            kernel.Bind<IProductCatalogService>().To
                     <ProductCatalogService>();

            // Product Catalogue & Category Service with Caching Layer Registration
            kernel.Bind<IProductCatalogService>().To<ProductCatalogService>();

            // Uncomment the line below to use the product service caching layer
            //kernel.Bind<IProductCatalogueService>().To<CachedProductCatalogueService>()
            //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

            kernel.Bind<IBasketService>().To
                     <BasketService>();
            kernel.Bind<ICookieStorageService>().To
                      <CookieStorageService>();


            // Application Settings                 
            kernel.Bind<IApplicationSettings>().To
                     <WebConfigApplicationSettings>();

            // Logger
            kernel.Bind<ILogger>().To
                      <Log4NetAdapter>();

            // Email Service                 
            kernel.Bind<IEmailService>().To
                    <TextLoggingEmailService>();

            kernel.Bind<ICustomerService>().To
                    <CustomerService>();

            // Authentication
            kernel.Bind<IExternalAuthenticationService>().To<JanrainAuthenticationService>();
            kernel.Bind<IFormsAuthentication>().To<AspFormsAuthentication>();
            kernel.Bind<ILocalAuthenticationService>().To<AspMembershipAuthentication>();

            // Controller Helpers
            kernel.Bind<IActionArguments>().To
                 <HttpRequestActionArguments>();
        }

        public class ControllerRegistry : Registry
        {
            public ControllerRegistry()
            {
                // Repositories 
                ForRequestedType<IOrderRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.OrderRepository>();
                ForRequestedType<ICustomerRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.CustomerRepository>();
                ForRequestedType<IBasketRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.BasketRepository>();
                ForRequestedType<IDeliveryOptionRepository>().TheDefault.Is.OfConcreteType
                          <Repository.NHibernate.Repositories.DeliveryOptionRepository>();
               
                ForRequestedType<ICategoryRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.CategoryRepository>();
                ForRequestedType<IProductTitleRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.ProductTitleRepository>();
                ForRequestedType<IProductRepository>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.Repositories.ProductRepository>();
                ForRequestedType<IUnitOfWork>().TheDefault.Is.OfConcreteType
                         <Repository.NHibernate.NHUnitOfWork>();

                // Order Service
                ForRequestedType<IOrderService>().TheDefault.Is.OfConcreteType
                        <OrderService>();

                // Payment
                ForRequestedType<IPaymentService>().TheDefault.Is.OfConcreteType
                        <PayPalPaymentService>();

                // Handlers for Domain Events
                ForRequestedType<IDomainEventHandlerFactory>().TheDefault
                       .Is.OfConcreteType<StructureMapDomainEventHandlerFactory>();
                ForRequestedType<IDomainEventHandler<OrderSubmittedEvent>>()
                       .AddConcreteType<OrderSubmittedHandler>();


                // Product Catalogue                                         
                ForRequestedType<IProductCatalogService>().TheDefault.Is.OfConcreteType
                         <ProductCatalogService>();

                // Product Catalogue & Category Service with Caching Layer Registration
                this.InstanceOf<IProductCatalogService>().Is.OfConcreteType<ProductCatalogService>()
                    .WithName("RealProductCatalogueService");

                // Uncomment the line below to use the product service caching layer
                //ForRequestedType<IProductCatalogueService>().TheDefault.Is.OfConcreteType<CachedProductCatalogueService>()
                //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

                ForRequestedType<IBasketService>().TheDefault.Is.OfConcreteType
                         <BasketService>();
                ForRequestedType<ICookieStorageService>().TheDefault.Is.OfConcreteType
                          <CookieStorageService>();


                // Application Settings                 
                ForRequestedType<IApplicationSettings>().TheDefault.Is.OfConcreteType
                         <WebConfigApplicationSettings>();

                // Logger
                ForRequestedType<ILogger>().TheDefault.Is.OfConcreteType
                          <Log4NetAdapter>();

                // Email Service                 
                ForRequestedType<IEmailService>().TheDefault.Is.OfConcreteType
                        <TextLoggingEmailService>();

                ForRequestedType<ICustomerService>().TheDefault.Is.OfConcreteType
                        <CustomerService>();

                // Authentication
                ForRequestedType<IExternalAuthenticationService>().TheDefault.Is
                         .OfConcreteType<JanrainAuthenticationService>();
                ForRequestedType<IFormsAuthentication>().TheDefault.Is
                         .OfConcreteType<AspFormsAuthentication>();
                ForRequestedType<ILocalAuthenticationService>().TheDefault.Is
                         .OfConcreteType<AspMembershipAuthentication>();

                // Controller Helpers
                ForRequestedType<IActionArguments>().TheDefault.Is.OfConcreteType
                     <HttpRequestActionArguments>();

            }
        }
    }
}
