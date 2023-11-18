using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer_Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        [RegularExpression(@"\d{10}", ErrorMessage = "Please enter a valid phone number!")]
        public long? Phone { get; set; }

    }
}