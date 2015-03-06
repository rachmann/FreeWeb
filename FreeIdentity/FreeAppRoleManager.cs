using FreeIdentity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Common;

namespace FreeIdentity
{

    public class FreeAppRoleManager : RoleManager<FreeAppRole,int>
    {
        public FreeAppRoleManager(IRoleStore<FreeAppRole, int> roleStore)
            : base(roleStore)
        {
        }


        public static FreeAppRoleManager Create(IdentityFactoryOptions<FreeAppRoleManager> options, IOwinContext context)
        {
            var manager = new FreeAppRoleManager(new FreeAppRoleStore(context.Get<DbConnection>()));

            return manager;
        }
    }
}
