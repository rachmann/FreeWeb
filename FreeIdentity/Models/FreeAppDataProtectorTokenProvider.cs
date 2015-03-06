using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FreeIdentity.Models
{
    class FreeAppDataProtectorTokenProvider<TFreeAppUser> : DataProtectorTokenProvider<FreeAppUser, int>
    {
        public FreeAppDataProtectorTokenProvider(IDataProtector protector)
            : base(protector)
        {

        }
    }
}
