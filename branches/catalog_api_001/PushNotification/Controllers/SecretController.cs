using System;
using System.Web.Mvc;
using Ninject;
using PushNotification.DataContext;
using PushNotification.Repository;

namespace PushNotification.Controllers
{
    public class SecretController : Controller
    {
        [Inject]
        public INewsRepository NewsRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNews(NewsLog newsLog)
        {
            newsLog.Id = Guid.NewGuid();
            newsLog.PostTime = DateTime.Now;

            NewsRepository.Create(newsLog);

            return View("Index", newsLog);
        }
    }
}