using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FreeIdentity.Models
{
    [Table("FreeAppUserLogin")]
    public class FreeAppUserLogin
    {
        [Key]
        public int UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
