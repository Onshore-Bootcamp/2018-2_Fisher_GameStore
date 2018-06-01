using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStoreMVC.Models;
using GameStoreTwo.Models;

namespace GameStoreMVC.Mapping
{
    public class UserMapper
    {
        public static UserPO MapDoToPo(UserDO from)
        {
            UserPO to = new UserPO();
            to.UserID = from.UserID;
            to.Firstname = from.Firstname;
            to.Lastname = from.Lastname;
            to.Username = from.Username;
            to.Password = from.Password;
            to.EmailAddress = from.EmailAddress;
            to.RoleID = from.RoleID;

            return to;
        }

        public static List<UserPO> MapDoToPo(List<UserDO> from)
        {
            List<UserPO> to = new List<UserPO>();

            if (from != null)
            {
                foreach (UserDO item in from)
                {
                    UserPO mappedItem = MapDoToPo(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }

        public static UserDO MapPoToDo(UserPO from)
        {
            UserDO to = new UserDO();
            to.UserID = from.UserID;
            to.Firstname = from.Firstname;
            to.Lastname = from.Lastname;
            to.Username = from.Username;
            to.Password = from.Password;
            to.EmailAddress = from.EmailAddress;
            to.RoleID = from.RoleID;

            return to;
        }

        public static List<UserDO> MapPoToDo(List<UserPO> from)
        {
            List<UserDO> to = new List<UserDO>();

            if (from != null)
            {
                foreach (UserPO item in from)
                {
                    UserDO mappedItem = MapPoToDo(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }
    }
}