using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Services;
using Agathas.Storefront.Controllers.ViewModels.ProductCatalog;
using Agathas.Storefront.Controllers.Controllers;
using StructureMap;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
     [AllowCrossSiteJson]
    public class ProductController : ApiController
    {
        private Controllers.Controllers.ProductController productController;


        public ProductController()
        {

            this.productController =
                new Controllers.Controllers.ProductController(ObjectFactory.GetInstance<IProductCatalogService>(), ObjectFactory.GetInstance<ICookieStorageService>());
        }

        [HttpGet]
        public ProductSearchResultView GetProductsByCategory(int categoryId)
        {
            System.Web.Mvc.ViewResult viewResult = (System.Web.Mvc.ViewResult)this.productController.GetProductsByCategory(categoryId);

            ProductSearchResultView productSearchResultView = (ProductSearchResultView)viewResult.ViewData.Model;
            return productSearchResultView;
        }
        [HttpGet]
        public ProductDetailView Detail(int id)
        {
            System.Web.Mvc.ViewResult viewResult = (System.Web.Mvc.ViewResult)this.productController.Detail(id);

            ProductDetailView productSearchResultView = (ProductDetailView)viewResult.ViewData.Model;
            return productSearchResultView;

        }
    }
}