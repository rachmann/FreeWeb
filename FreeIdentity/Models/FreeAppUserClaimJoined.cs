using System;
using Microsoft.AspNet.Identity;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FreeIdentity.Models
{
    // DO NOT user this for inserts - use FreeAppUserClaim
    // this is just for inner join select
    public class FreeAppUserClaimJoined
    {
        [Key]
        public int ClaimId { get; set; }
        public int UserId { get; set; }

        public int ClaimTypeId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimValueType { get; set; }
        public string Issuer { get; set; }

        public string ClaimTypeCode { get; set; }

    }
 
}

