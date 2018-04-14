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
using Agathas.Storefront.Controllers.ViewModels.CustomerAccount;
using Agathas.Storefront.Services.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
    [AllowCrossSiteJson]
    public class CustomerController : ApiController
    {
        private Controllers.Controllers.CustomerController customerController;
        public CustomerController()
        {

            this.customerController = new Controllers.Controllers.CustomerController(ObjectFactory.GetInstance<ICookieStorageService>(),
                                       ObjectFactory.GetInstance<ICustomerService>(), ObjectFactory.GetInstance<IFormsAuthentication>());
        }
        [HttpGet]
        public CustomerDetailView Detail(string token)
        {
            return customerController.DetailForAPI(token);

        }
        [HttpPost]
        public CustomerDetailView Detail()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            CustomerView body = JsonConvert.DeserializeObject<CustomerView>(content.Result);
            return customerController.SaveDetailsForAPI(body);
        }
        [HttpPost]
        public void AddDeliveryAddress()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            CustomerDetailView customerDetailView = JsonConvert.DeserializeObject<CustomerDetailView>(content.Result);
            this.customerController.AddDeliveryAddressForAPI(customerDetailView);
        }
        [HttpGet]
        public CustomerDeliveryAddressView EditDeliveryAddress(string deliveryAddressId, string customerId)
        {
            return this.customerController.EditDeliveryAddressForAPI(int.Parse(deliveryAddressId), customerId);
        }
        [HttpPost]
        public void EditDeliveryAddress(string customerId)
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            DeliveryAddressView deliveryAddressView = JsonConvert.DeserializeObject<DeliveryAddressView>(content.Result);
            this.customerController.EditDeliveryAddressForAPI(deliveryAddressView, customerId);
            return;
        }
    }
}