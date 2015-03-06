using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace FreeIdentity.Models
{
    public class FreeAppRoleMapper : ClassMapper<FreeAppRole>
    {

        public FreeAppRoleMapper()
        {
            Map(f => f.Id).Ignore();
            AutoMap();
        }
    }
}
