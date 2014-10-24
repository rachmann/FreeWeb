using System;
using System.ComponentModel.DataAnnotations;

namespace FreeModels
{
    public class Phone
    {
        public int PhoneId { get; set; }

        public int StudentId { get; set; }

        /// <summary>
        /// 1: Home phone
        /// 2: Office prohe
        /// 3: Cell phone
        /// </summary>
        public int PhoneType { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        public Phone()
        {
            RecordDate = DateTime.Now;            
        }
    }
}