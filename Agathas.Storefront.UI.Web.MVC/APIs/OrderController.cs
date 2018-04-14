using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Controllers.ViewModels.CustomerAccount;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
    [AllowCrossSiteJson]
    public class OrderController : ApiController
    {
        private Controllers.Controllers.OrderController orderController;

        public OrderController()
        {
            this.orderController = new Controllers.Controllers.OrderController(ObjectFactory.GetInstance<ICustomerService>(),
                                        ObjectFactory.GetInstance<IOrderService>(), ObjectFactory.GetInstance<IFormsAuthentication>(),
                                        ObjectFactory.GetInstance<ICookieStorageService>());
        }
        [HttpGet]
        public CustomersOrderSummaryView List()
        {
            return this.orderController.getCustomerOrderSummaryView();
        }

        [HttpGet]
        public CustomerOrderView Detail(int orderId)
        {
            return this.orderController.getCustomerOrderView(orderId);
        }
    }
}