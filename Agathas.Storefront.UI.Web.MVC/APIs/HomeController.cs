using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Controllers.ViewModels.ProductCatalog;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
   [AllowCrossSiteJson]
    public class HomeController : ApiController
    {
        private Controllers.Controllers.HomeController homeController;


        public HomeController()
        {
            
            this.homeController =
                new Controllers.Controllers.HomeController(ObjectFactory.GetInstance<IProductCatalogService>(), ObjectFactory.GetInstance<ICookieStorageService>());
        }

        [HttpGet]
        public HomePageView Index()
        {
            System.Web.Mvc.ViewResult viewResult = (System.Web.Mvc.ViewResult)this.homeController.Index();

            HomePageView homePageView = (HomePageView)viewResult.ViewData.Model;
            return homePageView;
        }
    }
}