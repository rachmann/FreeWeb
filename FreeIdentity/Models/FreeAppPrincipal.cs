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
using System.Security.Claims;

namespace FreeIdentity.Models
{
    public interface ICloudPrincipal : IPrincipal
    {
        bool IsManagement();
        bool IsStaff();
        bool IsClient();
    }

    public class FreeAppPrincipal :  ClaimsPrincipal, ICloudPrincipal
    {

        private FreeAppUserManager _userManager;
        private readonly IPrincipal _principal;

        public FreeAppPrincipal(IPrincipal user)
        {

            this._principal = user;
        }

        // because we are ClaimsPrincipal, we don't need this.
        //public bool IsInRole(string role)
        //{
        //    var user = AsyncHelpers.RunSync<CloudUser>(() => UserManager.FindByNameAsync(this.Identity.Name));

        //    var roles = UserManager.GetRoles(user.Id);
        //    return roles.Contains(role);
        //}

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
                    var connectionSetting = ConfigurationManager.ConnectionStrings["HTAuthConnection"];
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
            if (claims!= null && claims.Any())
            {
                claim = claims.FirstOrDefault(c => c.Type == type);
            }
            return (claim != null) ? claim.Value : string.Empty;
        }

        public bool IsManagement()
        {
            return (GetClaimValue("UserType") == "1");
        }

        public bool IsStaff()
        {
            return (GetClaimValue("UserType") == "2");
        }

        public bool IsClient()
        {
            return (GetClaimValue("UserType") == "3");
        }
    }
}
