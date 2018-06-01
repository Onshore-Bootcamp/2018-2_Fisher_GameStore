using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStoreTwo.Models;
using GameStoreBLL.Models;

namespace GameStoreMVC.Mapping
{
    public class GamesBOMapper
    {
        public static GamesBO MapDoToBo(GamesDO from)
        {
            GamesBO to = new GamesBO();
            to.GameID = from.GameID;
            to.GameName = from.GameName;
            to.System = from.System;
            to.Genre = from.Genre;
            to.ReleaseDate = from.ReleaseDate;
            to.Price = from.Price;

            return to;
        }

        public static List<GamesBO> MapDoToBo(List<GamesDO> from)
        {
            List<GamesBO> to = new List<GamesBO>();

            if (from != null)
            {
                foreach (GamesDO item in from)
                {
                    GamesBO mappedItems = MapDoToBo(item);
                    to.Add(mappedItems);
                }
            }
            return to;
        }

        public static GamesDO MapBoToDo(GamesBO from)
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

        public static List<GamesDO> MapBoToDo(List<GamesBO> from)
        {
            List<GamesDO> to = new List<GamesDO>();

            if (from != null)
            {
                foreach (GamesBO item in from)
                {
                    GamesDO mappedItems = MapBoToDo(item);
                    to.Add(mappedItems);
                }
            }
            return to;
        }
    }
}