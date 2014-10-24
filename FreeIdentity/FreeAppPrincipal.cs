using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using FreeIdentity.Models;
using System.Security.Claims;

namespace FreeIdentity
{
    public interface IFreeAppPrincipal : IPrincipal
    {
        bool IsAdmin();
        bool IsStaff();
        bool IsCustomer();
    }

    public class FreeAppPrincipal :  ClaimsPrincipal, IFreeAppPrincipal
    {

        private FreeAppUserManager _userManager;
        private readonly IPrincipal _principal;

        public FreeAppPrincipal(IPrincipal user)
        {

            this._principal = user;
        }

        public new IIdentity Identity
        {
            get { return _principal.Identity; }
        }

        public FreeAppUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    var connectionSetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                    _userManager = new FreeAppUserManager(new FreeAppUserStore(new SqlConnection(connectionSetting.ConnectionString)));
                }

                return _userManager;
            }
            set { _userManager = value; }
        }
        private string GetClaimValue(string type)
        {
            var claims = ((ClaimsIdentity)_principal.Identity).Claims;
            Claim claim = null;
            if (claims.Any())
            {
                claim = claims.FirstOrDefault(c => c.Type == type);
            }
            return (claim != null) ? claim.Value : string.Empty;
        }
        public bool IsAdmin()
        {
            return (GetClaimValue("UserType") == "1");
        }
        public bool IsStaff()
        {
            return (GetClaimValue("UserType") == "2");
        }

        public bool IsCustomer()
        {
            return (GetClaimValue("UserType") == "3");
        }
    }
}
