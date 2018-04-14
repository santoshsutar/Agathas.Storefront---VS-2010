using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StructureMap;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Services.Interfaces;
using Agathas.Storefront.Controllers.ActionArguments;
using Agathas.Storefront.Controllers.ViewModels.Account;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Agathas.Storefront.UI.Web.MVC.APIs
{
     [AllowCrossSiteJson]
    public class AccountLogOnController : ApiController
    {
        private Controllers.Controllers.AccountLogOnController accountLogOnController;

        public AccountLogOnController()
        {
            this.accountLogOnController = new Controllers.Controllers.AccountLogOnController(ObjectFactory.GetInstance<ILocalAuthenticationService>(),
                ObjectFactory.GetInstance<ICustomerService>(), ObjectFactory.GetInstance<IExternalAuthenticationService>(),
                ObjectFactory.GetInstance<IFormsAuthentication>(), ObjectFactory.GetInstance<IActionArguments>());
        }
       
        [HttpPost]
        public Agathas.Storefront.Infrastructure.Authentication.User LogOn()
        {
            Task<string> content = this.Request.Content.ReadAsStringAsync();
            User body = JsonConvert.DeserializeObject<User>(content.Result);
            Agathas.Storefront.Infrastructure.Authentication.User user = this.accountLogOnController.logOnForAPI(body.Email, body.Password);
            return user;
        }
        

    }
     public class User
     {
         public string Email;
         public string Password;
         
     }
}