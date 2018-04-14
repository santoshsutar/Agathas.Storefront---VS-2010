using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Infrastructure.Payments;
using Agathas.Storefront.Services.Interfaces;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
     [AllowCrossSiteJson]
    public class PaymentController : ApiController
    {
         private Controllers.Controllers.PaymentController paymentController;
         public PaymentController()
         {
             this.paymentController = new Controllers.Controllers.PaymentController(ObjectFactory.GetInstance<IPaymentService>(),
                                        ObjectFactory.GetInstance<IOrderService>());
         }
         [HttpGet]
         public PaymentPostData CreatePaymentFor(int orderId)
         {
            return this.paymentController.CreatePaymentPostData(orderId);
         }
    }
}