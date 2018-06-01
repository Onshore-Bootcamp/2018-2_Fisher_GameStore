using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameStoreMVC.Models
{
    public class GamesPO
    {
        //Declaring all object properties for game
        public Int64 GameID { get; set; }

        [Required]
        [StringLength(60)]
        public string GameName { get; set; }

        [Required]
        [StringLength(40)]
        public string System { get; set; }

        [Required]
        [StringLength(75)]
        public string Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}