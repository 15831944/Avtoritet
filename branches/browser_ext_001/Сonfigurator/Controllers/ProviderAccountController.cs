using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class ProviderAccountController : Controller
    {
        // GET: ProviderAccount
        public ActionResult Index(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {

                var providerAcc = ae.ProviderAccount.Where(t => t.ProviderId == providerId).OrderBy(t => t.Order).ToList();
                ViewBag.providerId = providerId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForProviderAccount(providerId);
                return View(providerAcc);
            }
        }

        // GET: ProviderAccount/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: ProviderAccount/Edit/5
        public ActionResult Edit(int providerAccId=0,int providerId=0)
        {
            using (var ae = new AvtoritetEntities())
            {
                var providerAcc = ae.ProviderAccount.FirstOrDefault(t => t.ProviderAccountId == providerAccId) ?? new ProviderAccount();
                ViewBag.ProviderId = providerId;
                return View(providerAcc);
            }
        }

        // POST: ProviderAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int providerAccId,int providerId,bool enable,FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    ProviderAccount providerAcc = null;
                    if (providerAccId == 0)
                    {
                        providerAcc = new ProviderAccount
                        {
                            Uri = collection["Uri"],
                            Login = collection["Login"],
                            Password = collection["Password"],
                            ProviderId = providerId,
                            Enable = enable,
                            Order = !string.IsNullOrEmpty(collection["order"]) ? int.Parse(collection["order"]) : 0
                        };
                        ae.ProviderAccount.Add(providerAcc);
                    }
                    else
                    {
                        providerAcc = ae.ProviderAccount.FirstOrDefault(t => t.ProviderId == providerId);
                        if (providerAcc != null)
                        {
                            providerAcc.Uri = collection["Uri"];
                            providerAcc.Login = collection["Login"];
                            providerAcc.Password = collection["Password"];
                            providerAcc.Enable = enable;
                            providerAcc.Order = !string.IsNullOrEmpty(collection["order"])
                                ? int.Parse(collection["order"])
                                : 0;
                        }
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index", new { ProviderId = providerId });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: ProviderAccount/Delete/5
        public ActionResult Delete(int id,int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var providerAcc = ae.ProviderAccount.FirstOrDefault(t => t.ProviderAccountId == id);
                if (providerAcc != null)
                {
                    ae.ProviderAccount.Remove(providerAcc);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new { ProviderId = providerId });
            }
        }
    }
}
