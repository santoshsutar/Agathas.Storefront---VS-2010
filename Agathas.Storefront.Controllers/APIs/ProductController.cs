using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Controllers.ViewModels.ProductCatalog;
using StructureMap;


namespace Agathas.Storefront.Controllers.APIs
{
    public class ProductController1 : ApiController
    {
         private Controllers.ProductController productController;

        
        public ProductController1()
        {
            
            this.productController =
                new Controllers.ProductController(ObjectFactory.GetInstance<IProductCatalogService>(), ObjectFactory.GetInstance<ICookieStorageService>());
        }

        [HttpGet]
        public ProductSearchResultView GetProductsByCategory(int categoryId)
        {
            System.Web.Mvc.ViewResult viewResult = (System.Web.Mvc.ViewResult)this.productController.GetProductsByCategory(categoryId);

            ProductSearchResultView productSearchResultView = (ProductSearchResultView)viewResult.ViewData.Model;
            return productSearchResultView;
        }

    }
}