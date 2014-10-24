 using Dapper.Contrib.Extensions;
using Microsoft.AspNet.Identity;

namespace FreeIdentity.Models
{
    [Table("FreeAppRole")]
    public class FreeAppRole : IRole<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
