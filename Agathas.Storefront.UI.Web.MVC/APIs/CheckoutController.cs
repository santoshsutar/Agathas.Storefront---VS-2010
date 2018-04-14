using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Controllers.ViewModels.Checkout;
using Agathas.Storefront.Services.Messaging.OrderService;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Agathas.Storefront.Controllers;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
      [AllowCrossSiteJson]
    public class CheckoutController : ApiController
    {
        private Controllers.Controllers.CheckoutController checkoutController;
        public CheckoutController()
        {
            this.checkoutController = new Controllers.Controllers.CheckoutController(ObjectFactory.GetInstance<ICookieStorageService>(),
                                        ObjectFactory.GetInstance<IBasketService>(), ObjectFactory.GetInstance<ICustomerService>(),
                                        ObjectFactory.GetInstance<IOrderService>(), ObjectFactory.GetInstance<IFormsAuthentication>());
        }
        [HttpGet]
        public OrderConfirmationView Checkout()
        {
            OrderConfirmationView orderConfirmationView = this.checkoutController.getOrderConfirmationView();
            return orderConfirmationView;
        }
        [HttpPost]
        public int PlaceOrder()
        {
            System.Web.Mvc.FormCollection collection = new System.Web.Mvc.FormCollection();
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            int body = JsonConvert.DeserializeObject<int>(content.Result);
            collection.Add(FormDataKeys.DeliveryAddress.ToString(), body.ToString());
            CreateOrderResponse createOrderResponse = this.checkoutController.CreateOrderResponse(collection);
            return createOrderResponse.Order.Id;
        }
    }
}