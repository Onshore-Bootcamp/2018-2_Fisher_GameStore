using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameStoreMVC.Models
{
    public class OrderPO
    {
        //Declaring all object properties for order
        public Int64 OrderID { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name="Shipping Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 15 characters.")]
        [Range(0, 999999999999999)]
        public string Phone { get; set; }

        [StringLength(10, MinimumLength = 5, ErrorMessage = "Zip code must be between 5 and 10 characters.")]
        [Range(0, 9999999999)]
        [Required]
        public string ZipCode { get; set; }

        public Int64 UserID { get; set; }
    }
}