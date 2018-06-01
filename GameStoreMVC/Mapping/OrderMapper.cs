using GameStoreMVC.Models;
using GameStoreTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStoreMVC.Mapping
{
    public class OrderMapper
    {
        public static OrderPO MapDoToPo(OrdersDO from)
        {
            OrderPO to = new OrderPO();
            to.OrderID = from.OrderID;
            to.Name = from.Name;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.ZipCode = from.ZipCode;
            to.UserID = from.UserID;

            return to;
        }

        public static List<OrderPO> MapDoToPo(List<OrdersDO> from)
        {
            List<OrderPO> to = new List<OrderPO>();

            if (from != null)
            {
                foreach (OrdersDO item in from)
                {
                    OrderPO mappedItem = MapDoToPo(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }

        public static OrdersDO MapPoToDo(OrderPO from)
        {
            OrdersDO to = new OrdersDO();
            to.OrderID = from.OrderID;
            to.Name = from.Name;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.ZipCode = from.ZipCode;
            to.UserID = from.UserID;

            return to;
        }

        public static List<OrdersDO> MapPoToDo(List<OrderPO> from)
        {
            List<OrdersDO> to = new List<OrdersDO>();

            if (from != null)
            {
                foreach (OrderPO item in from)
                {
                    OrdersDO mappedItem = MapPoToDo(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }
    }
}