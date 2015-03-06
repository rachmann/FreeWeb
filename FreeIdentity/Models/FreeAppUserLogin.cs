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
    public class FreeAppUserLogin
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public string LoginProvider { get; set; }
        [Key]
        public string ProviderKey { get; set; }
    }
}
