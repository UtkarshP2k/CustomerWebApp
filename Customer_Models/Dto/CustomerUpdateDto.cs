using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Models.Dto
{
    public class CustomerUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        [RegularExpression(@"\d{10}", ErrorMessage = "Please enter a valid phone number!")]
        public long? Phone { get; set; }
    }
}
