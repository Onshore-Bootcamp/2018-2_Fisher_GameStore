namespace GameStoreMVC.Models
{
    using System.Web;
    using System.Web.Mvc;


    public class FilterClass : ActionFilterAttribute
    {
        //Before Method is ran
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            base.OnActionExecuting(filterContext);
        }
    }
}