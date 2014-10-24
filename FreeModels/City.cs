using System.ComponentModel.DataAnnotations;

namespace FreeModels
{
    public class City
    {
        public int CityId { get; set; }

        [Required]
        public string CityName { get; set; }
    }
}