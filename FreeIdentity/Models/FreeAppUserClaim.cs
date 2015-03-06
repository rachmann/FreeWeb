using System;
using Dapper;
using DapperExtensions;
using FreeIdentity.DapperExtensions;
using Microsoft.AspNet.Identity;

namespace FreeIdentity.Models
{
    public class FreeAppUserClaim
    {
        [Key]
        public int ClaimId { get; set; }
        public int UserId { get; set; }

        public int ClaimTypeId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimValueType { get; set; }
        public string Issuer { get; set; }
    }
 
}
