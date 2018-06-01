using GameStoreTwo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStoreMVC.Models;
using GameStoreTwo.Models;
using GameStoreMVC.Mapping;
using GameStoreBLL;
using GameStoreBLL.Models;

namespace GameStoreMVC.Controllers
{
    public class GameController : Controller
    {
        private GamesBLO businessLayer;

        private GamesDAO dataAccess;
        public GameController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new GamesDAO(connectionString, filePath);
            businessLayer = new GamesBLO();
        }
        //Dependencies

        private string connectionString;

        
        [HttpGet]
        public ActionResult Index()
        {
            List<GamesPO> mappedItem = new List<GamesPO>();
            try
            {
                //Mapping in a dataAccess call
                List<GamesDO> dataObject = dataAccess.ViewAllGames();
                mappedItem = GameMapper.MapDoToPo(dataObject);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("Message"))
                {
                    TempData["Message"] = "No Games Found";
                }
            }
            return View(mappedItem);
        }

        [HttpGet]
        public ActionResult CreateGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(GamesPO form)
        {
            //Declaring local variables
            ActionResult oResponse = RedirectToAction("Index", "Game");

            //Checking if Model state is Valid
            if (ModelState.IsValid)
            {
                try
                {
                    //Mapping in a dataAccess call
                    GamesDO dataObject = GameMapper.MapPoToDo(form);
                    dataAccess.CreateNewGame(dataObject);

                    TempData["Message"] = $"{form.GameName} was created successfully";
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

            return oResponse;
        }

        [HttpGet]
        //Method that gets the information for UpdateGame
        public ActionResult UpdateGame(Int64 gameId)
        {
            GamesPO displayObject = new GamesPO();
            try
            {
                //Mapping in a dataAccess call
                GamesDO game = dataAccess.ViewGameAtID(gameId);
                displayObject = GameMapper.MapDoToPo(game);
            }
            catch (Exception ex)
            {


            }
            return View(displayObject);
        }

        [HttpPost]
        //Method that posts the information for UpdateGame
        public ActionResult UpdateGame(GamesPO form)
        {
            //Declaring local variables
            ActionResult oResponse = RedirectToAction("Index", "Game");

            //Checking if Model state is Valid
            if (ModelState.IsValid)
            {
                try
                {
                    //Mapping in a dataAccess call
                    GamesDO dataObject = GameMapper.MapPoToDo(form);
                    dataAccess.UpdateGames(dataObject);
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
            return oResponse;
        }

        [HttpGet]
        public ActionResult DeleteGame(Int64 gameId)
        {
            //Declaring local variables
            ActionResult oResponse = RedirectToAction("Index", "Game");
            try
            {
                dataAccess.DeleteGames(gameId);
            }
            catch (Exception ex)
            {

            }
            return oResponse;
        }

        [HttpGet]
        //Method that Adds an Item to the cart.
        public ActionResult AddItemToCart(int gameId)
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<int>();
            }
            ((List<int>)Session["Cart"]).Add(gameId);

            return RedirectToAction("Index", "Game");
        }

        [HttpGet]
        //Method that Removes an Item from the cart.
        public ActionResult RemoveItemFromCart(int gameId)
        {
            ActionResult oReponse = RedirectToAction("Index", "Game");
            if (Session["Cart"] != null)
            {
                ((List<int>)Session["Cart"]).Remove(gameId);
                oReponse = RedirectToAction("Cart", "Game");
            }

            return oReponse;
        }

        [HttpGet]
        //Method that creates a Cart 
        public ActionResult Cart()
        {
            List<GamesPO> displayGames = new List<GamesPO>();
            try
            {
                List<GamesBO> businessObjects = new List<GamesBO>();
                List<int> cart = (List<int>)Session["Cart"];
                foreach (int gameId in cart)
                {
                    GamesDO games = dataAccess.ViewGameAtID(gameId);
                    displayGames.Add(GameMapper.MapDoToPo(games));
                    businessObjects.Add(GamesBOMapper.MapDoToBo(games));
                }
                decimal totalSum = businessLayer.CalculationSum(businessObjects);
                ViewBag.Total = totalSum;
            }
            catch (Exception ex)
            {

            }

            return View(displayGames);
        }

    }
}