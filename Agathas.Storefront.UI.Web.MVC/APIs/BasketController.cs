using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Controllers.ViewModels;
using Agathas.Storefront.Controllers.ViewModels.ProductCatalog;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Agathas.Storefront.Controllers.JsonDTOs;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
    [AllowCrossSiteJson]
    public class BasketController : ApiController
    {
        private Controllers.Controllers.BasketController basketController;

        public BasketController()
        {
            this.basketController = new Controllers.Controllers.BasketController(ObjectFactory.GetInstance<IProductCatalogService>(),
                                     ObjectFactory.GetInstance<IBasketService>(), ObjectFactory.GetInstance<ICookieStorageService>());
        }
        [HttpGet]
        public BasketSummaryView AddToBasket(int productId)
        {

            return (BasketSummaryView) basketController.AddToBasket(productId).Data;
        }
        [HttpGet]
        public BasketSummaryView getBasketSummaryView()
        {
            return this.basketController.GetBasketSummaryView();
        }
        [HttpGet]
        public BasketDetailView Detail()
        {
            var viewResult = (System.Web.Mvc.ViewResult)this.basketController.Detail();
            return (BasketDetailView)viewResult.ViewData.Model;
        }
        [HttpPost]
        public BasketDetailView RemoveItem()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<int>(content.Result);
            return (BasketDetailView)this.basketController.RemoveItem(body).Data;
        }
        [HttpPost]
        public BasketDetailView UpdateItems()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<JsonBasketQtyUpdateRequest>(content.Result);
            return (BasketDetailView)this.basketController.UpdateItems(body).Data;
        }
        [HttpPost]
        public BasketDetailView UpdateShipping()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<int>(content.Result);
            return (BasketDetailView)this.basketController.UpdateShipping(body).Data;
        }
    }
}