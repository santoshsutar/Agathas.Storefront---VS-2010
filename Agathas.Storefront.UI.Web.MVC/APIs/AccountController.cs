using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Controllers.ActionArguments;
using Agathas.Storefront.Services.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Agathas.Storefront.Controllers;


namespace Agathas.Storefront.UI.Web.MVC.APIs
{
    [AllowCrossSiteJson]
    public class AccountController : ApiController
    {
        private Controllers.Controllers.AccountRegisterController accountRegisterController;

        public AccountController()
        {
            accountRegisterController = new Controllers.Controllers.AccountRegisterController(ObjectFactory.GetInstance<ILocalAuthenticationService>(),
                                        ObjectFactory.GetInstance<ICustomerService>(), ObjectFactory.GetInstance<IExternalAuthenticationService>(),
                                        ObjectFactory.GetInstance<IFormsAuthentication>(), ObjectFactory.GetInstance<IActionArguments>());
        }
        [HttpPost]
        public bool Register()
        {

            System.Web.Mvc.FormCollection collection = new System.Web.Mvc.FormCollection();
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            AccountRegister body = JsonConvert.DeserializeObject<AccountRegister>(content.Result);
            collection.Add(FormDataKeys.Password.ToString(), body.Password);
            collection.Add(FormDataKeys.Email.ToString(), body.Email);
            collection.Add(FormDataKeys.FirstName.ToString(), body.FirstName);
            collection.Add(FormDataKeys.SecondName.ToString(), body.SecondName);
            System.Web.Mvc.ActionResult viewResult = (System.Web.Mvc.ActionResult)accountRegisterController.Register(collection);
            if (viewResult is System.Web.Mvc.ViewResult)
            {
                return false;// same page User already exists
            }
            else if (viewResult is System.Web.Mvc.RedirectToRouteResult)
            {
                return true;//direct to next page new user
            }
            return false;
        }
    }
    public class AccountRegister
    {
        public string FirstName;
        public string SecondName;
        public string Email;
        public string Password;
    }
}
