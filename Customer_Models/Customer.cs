﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer_Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.PhoneNumber,ErrorMessage = "Please enter a valid phone number!")]
        public long? Phone { get; set; }

    }
}