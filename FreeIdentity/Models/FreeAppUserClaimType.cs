﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace FreeIdentity.Models
{
    public class FreeAppUserClaimType
    {
        [Key]
        public int TypeId { get; set; }
        public string ClaimTypeCode { get; set; }
        public string ClaimTypeDescription { get; set; }
    }
}
