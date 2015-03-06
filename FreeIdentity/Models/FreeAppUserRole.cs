using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using FreeIdentity.DapperExtensions;

namespace FreeIdentity.Models
{
    public class FreeAppUserRole
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int UserId { get; set; }

        public virtual FreeAppRole FreeAppRole { get; set; }
        public virtual FreeAppUser FreeAppUser { get; set; }
    }
}
