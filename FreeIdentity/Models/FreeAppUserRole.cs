using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace FreeIdentity.Models
{
    [Table("FreeAppUserRole")]
    public class FreeAppUserRole
    {
        [Key]
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
