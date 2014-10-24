using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using FreeModels;
using FreeIdentity;
using FreeIdentity.Models;


namespace FreeWeb
{
    
    public class ApplicationSignInManager : SignInManager<FreeAppUser, int>
    {
        public ApplicationSignInManager(FreeAppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(FreeAppUser user)
        {
            return user.GenerateUserIdentityAsync((FreeAppUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<FreeAppUserManager>(), context.Authentication);
        }
    }


  
}
