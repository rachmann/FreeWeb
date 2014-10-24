using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using FreeIdentity.Models;

namespace FreeIdentity
{
    class FreeAppProtectorTokenProvider<TFreeAppUser> : DataProtectorTokenProvider<FreeAppUser, int>
    {
        public FreeAppProtectorTokenProvider(IDataProtector protector) : base(protector)
        {

        }
    }
}
