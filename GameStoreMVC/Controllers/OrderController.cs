using GameStoreMVC.Mapping;
using GameStoreMVC.Models;
using GameStoreTwo;
using GameStoreTwo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStoreMVC.Controllers;

namespace GameStoreMVC.Controllers
{
    [FilterClass]
    public class OrderController : Controller
    {
        private OrdersDAO dataAccess;
        private GameController controllerAccess;
        public OrderController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new OrdersDAO(connectionString, filePath);
        }

        private string connectionString; 

        // GET: Order
        [HttpGet]
        public ActionResult Index()
        {
            List<OrderPO> mappedItems = new List<OrderPO>();
            try
            {
                List<OrdersDO> dataObjects = dataAccess.ViewAllOrders();
                mappedItems = OrderMapper.MapDoToPo(dataObjects);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("Message"))
                {
                    TempData["Message"] = "No Orders";
                }

            }

            return View(mappedItems);
        }

        [HttpGet]
        public ActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Create(OrderPO form)
        {
            
            ActionResult oResponse = RedirectToAction("Index", "Game");

            if (ModelState.IsValid)
            {
                try
                {      
                    form.UserID = (Int64)Session["UserID"];
                    OrdersDO dataObject = OrderMapper.MapPoToDo(form);
                    dataAccess.CreateNewOrder(dataObject,Session["Cart"] as List<int>);

                    TempData["Message"] = $"{form.Name} order has been created";
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                }
            }
            else
            {
                oResponse = View(form);
            }
            Session["Cart"] = null;
            return oResponse;
        }

        [HttpGet]
        public ActionResult UpdateOrder(Int64 orderID)
        {
            OrderPO displayObject = new OrderPO();
            try
            {
                OrdersDO item = dataAccess.ViewOrdersByID(orderID);
                displayObject = OrderMapper.MapDoToPo(item);
            }
            catch (Exception ex)
            {

            }
            return View(displayObject);
        }

        [HttpPost]
        public ActionResult UpdateOrder(OrderPO form)
        {
            ActionResult oReponse = RedirectToAction("Index", "Order");

            if (ModelState.IsValid)
            {
                try
                {
                    form.UserID = (Int64)Session["UserID"];
                    OrdersDO dataObject = OrderMapper.MapPoToDo(form);
                    dataAccess.UpdateOrder(dataObject);
                }
                catch (Exception ex)
                {
                    oReponse = View(form);
                }

            }
            else
            {
                oReponse = View(form);
            }
            return oReponse;
        }

        [HttpGet]
        public ActionResult DeleteOrder(Int64 orderId)
        {
            ActionResult oResponse = RedirectToAction("Index", "Order");
            try
            {
                dataAccess.DeleteOrder(orderId);
            }
            catch (Exception ex)
            {

                
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult OrderDetails(Int64 orderId)
        {
            ViewBag.Orders = dataAccess.ViewOrdersByID(orderId);
            return View();
        }
    }
}
