using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace FreeIdentity.Models
{
    public class FreeAppUserRole
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int UserId { get; set; }
    }
}
