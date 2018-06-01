using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameStoreMVC.Models
{
    public class UserPO
    {
        //Declaring all object properties for user
        public Int64 UserID { get; set; }

        [Required]
        [StringLength(40)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Please enter a Username between 5-50 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Please enter a Password between 5-25 characters")]
        [RegularExpression("^([.+A-Z][a-z]{5,25})",ErrorMessage ="Please enter a password between 5-25 characters and at least one Uppercase Letter")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string EmailAddress { get; set; }


        public Int64 RoleID { get; set; }
    }
}