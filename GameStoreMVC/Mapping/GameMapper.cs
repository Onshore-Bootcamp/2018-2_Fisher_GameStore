using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStoreMVC.Models;
using GameStoreTwo.Models;

namespace GameStoreMVC.Mapping
{
    public class GameMapper
    {
        //Mapping GamesDo to GamesPO
        public static GamesPO MapDoToPo(GamesDO from)
        {
            GamesPO to = new GamesPO();
            to.GameID = from.GameID;
            to.GameName = from.GameName;
            to.System = from.System;
            to.Genre = from.Genre;
            to.ReleaseDate = from.ReleaseDate;
            to.Price = from.Price;

            return to;
        }

        public static List<GamesPO> MapDoToPo(List<GamesDO> from)
        {
            List<GamesPO> to = new List<GamesPO>();

            if (from != null)
            {
                foreach (GamesDO item in from)
                {
                    GamesPO mappedItems = MapDoToPo(item);
                    to.Add(mappedItems);
                }
            }
            return to;
        }

        //Mapping GamesPO to GamesDO
        public static GamesDO MapPoToDo(GamesPO from)
        {
            GamesDO to = new GamesDO();
            to.GameID = from.GameID;
            to.GameName = from.GameName;
            to.System = from.System;
            to.Genre = from.Genre;
            to.ReleaseDate = from.ReleaseDate;
            to.Price = from.Price;

            return to;
        }

        public static List<GamesDO> MapPoToDo(List<GamesPO> from)
        {
            List<GamesDO> to = new List<GamesDO>();

            if (from != null)
            {
                foreach (GamesPO item in from)
                {
                    GamesDO mappedItems = MapPoToDo(item);
                    to.Add(mappedItems);
                }
            }
            return to;
        }
    }
}