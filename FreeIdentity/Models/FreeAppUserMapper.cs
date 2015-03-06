using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using FreeIdentity.DapperExtensions;

namespace FreeIdentity.Models
{
    public class FreeAppUserMapper : ClassMapper<FreeAppUser>
    {

        public FreeAppUserMapper()
        {
            Map(f => f.Id).Ignore();
            AutoMap();
        }
    }
}
