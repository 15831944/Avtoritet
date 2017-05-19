using System.Web.Mvc;

namespace PushNotification.Controllers
{
    public class HubController : Controller
    {
        //
        // GET: /Hub/
        public ActionResult Index()
        {
            return View();
        }
    }
}