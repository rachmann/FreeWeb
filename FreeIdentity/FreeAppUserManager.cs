using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNet.Identity;
using FreeIdentity.Models;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

// use
// http://www.asp.net/identity/overview/extensibility/change-primary-key-for-users-in-aspnet-identity

namespace FreeIdentity
{
    public class FreeAppUserManager : UserManager<FreeAppUser, int>
    {
        public FreeAppUserManager(IUserStore<FreeAppUser, int> store)
            : base(store)
        {
            
        }

        //public static FreeAppUserManager Create(string connectionString)
        //{
        //    IUserStore<FreeAppUser, int> store = new FreeAppUserStore<FreeAppUser>(connectionString);
        //    return new FreeAppUserManager(store);

        //}
        public static FreeAppUserManager Create(IdentityFactoryOptions<FreeAppUserManager> options, IOwinContext context)
        {

            // var manager = new FreeAppUserManager(new FreeAppUserStore(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()));

            // now depend on OwinContext as setup in Statup.ConfigureAuth in App_Start\Startup.Auth.cs
            var manager = new FreeAppUserManager(new FreeAppUserStore(context.Get<DbConnection>()));

            manager.UserValidator = new UserValidator<FreeAppUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
               
            };

            manager.ClaimsIdentityFactory = new FreeAppClaimsIdentityFactory();
            manager.EmailService = new EmailService();
            manager.RegisterTwoFactorProvider("PhoneCode",
            new PhoneNumberTokenProvider<FreeAppUser, int>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<FreeAppUser, int>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                }); 
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<FreeAppUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public List<FreeAppUserClaimType> GetClaimTypes()
        {
            return (List<FreeAppUserClaimType>)((FreeAppUserStore)Store).GetClaimTypes().Result;

        }
   
    }
}
