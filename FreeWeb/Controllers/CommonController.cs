using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using FreeIdentity;

namespace FreeWeb.Controllers
{
    public class CommonController : Controller
    {
        private FreeAppUserManager _userManager;

        public FreeAppUserManager UserManager
        {
            get
            {
                return _userManager ??
                       (_userManager =
                           System.Web.HttpContext.Current.Request.GetOwinContext().GetUserManager<FreeAppUserManager>());
            }
            set { _userManager = value; }
        }

        public virtual new IFreeAppPrincipal User
        {
            get { return (IFreeAppPrincipal)base.User; }
        }

        public string DbConnection
        {
            get { return (string)Session["companyConnection"]; }
            set { Session["companyConnection"] = value; }
        }


    }

}