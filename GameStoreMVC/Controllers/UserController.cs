using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStoreTwo;
using GameStoreTwo.Models;
using GameStoreMVC.Mapping;
using GameStoreMVC.Models;
using System.Configuration;
using System.Web.Security;

namespace GameStoreMVC.Controllers
{
    public class UserController : Controller
    {
        private UserDAO dataAccess;
        private RolesDAO dataAccessRoles = new RolesDAO();
        public UserController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string filePath = ConfigurationManager.AppSettings["logPath"];
            dataAccess = new UserDAO(connectionString, filePath);
        }

        private string connectionString;

        [HttpGet]
        public ActionResult Index()
        {
            List<UserPO> mappedUsers = new List<UserPO>();
            try
            {
                List<UserDO> dataObjects = dataAccess.ViewAllUsers();
                mappedUsers = UserMapper.MapDoToPo(dataObjects);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("Message"))
                {
                    TempData["Message"] = "No Games Listed";
                }

            }

            return View(mappedUsers);
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginPO form)
        {
            ActionResult oResponse = RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                try
                {
                    UserDO storedUser = dataAccess.ReadUserByUsername(form.Username);
                    if (storedUser != null && form.Password.Equals(storedUser.Password))
                    {
                        Session["RoleID"] = storedUser.RoleID;
                        Session["Username"] = storedUser.Username;
                        Session["UserID"] = storedUser.UserID;
                    }
                    else
                    {
                        oResponse = View(form);
                        ModelState.AddModelError("Password", "Username or Password is incorrect");
                    }
                }
                catch (Exception ex)
                {
                    oResponse = View(form);
                    ModelState.AddModelError("Password", "Username or Password is incorrect");
                }
            }
            else
            {
                ModelState.AddModelError("Password", "Username or Password is incorrect");
                oResponse = View(form);
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserPO form)
        {
            ActionResult oResponse = RedirectToAction("Login", "User");

            if (ModelState.IsValid)
            {
                try
                {
                    form.RoleID = 3;
                    UserDO dataObject = UserMapper.MapPoToDo(form);
                    dataAccess.CreateUser(dataObject);

                    TempData["Message"] = "New Account Created";
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult UpdateUser(Int64 userID)
        {
            UserPO displayObject = new UserPO();
            try
            {
                List<RolesDO> roles = dataAccessRoles.ViewAllRoles();
                ViewBag.DropDownList = new List<SelectListItem>();
                //Turn all the roles into a option.
                foreach (RolesDO role in roles)
                {
                    ViewBag.DropDownList.Add(new SelectListItem() { Text = role.RoleName, Value = role.RoleID.ToString() });
                }
                

                UserDO item = dataAccess.ViewUserAtID(userID);
                displayObject = UserMapper.MapDoToPo(item);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(displayObject);
        }

        [HttpPost]
        public ActionResult UpdateUser(UserPO form)
        {
            ActionResult oResponse = RedirectToAction("Index", "User");

            if (ModelState.IsValid)
            {
                try
                {
                    UserDO dataObject = UserMapper.MapPoToDo(form);
                    dataAccess.UpdateUser(dataObject);
                }
                catch (Exception ex)
                {

                    oResponse = View(form);
                }
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult DeleteUser(Int64 userID)
        {
            ActionResult oResponse = RedirectToAction("Index", "User");
            if (dataAccess.ViewUserAtID(userID).RoleID != 1)
            {
                try
                {
                    dataAccess.DeleteUser(userID);
                }
                catch (Exception ex)
                {


                }
                
            }
            return oResponse;
        }
    }
}