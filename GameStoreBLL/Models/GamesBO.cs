using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL.Models
{
    public class GamesBO
    {
        //Declaring all object properties for game
        public Int64 GameID { get; set; }
        
        public string GameName { get; set; }
        
        public string System { get; set; }
        
        public string Genre { get; set; }
        
        public DateTime ReleaseDate { get; set; }

        public decimal Price { get; set; }
    }
}
