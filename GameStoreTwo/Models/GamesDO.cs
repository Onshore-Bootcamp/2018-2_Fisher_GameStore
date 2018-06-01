using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTwo.Models
{
    public class GamesDO
    {
        //Declaring all object fields for games
        public Int64 GameID;
        public string GameName;
        public string System;
        public string Genre;
        public DateTime ReleaseDate;
        public decimal Price;
    }
}
