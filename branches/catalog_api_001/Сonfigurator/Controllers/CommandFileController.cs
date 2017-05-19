using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class CommandFileController : Controller
    {
        public ActionResult Index(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var commandFiles = ae.CommandFile.Where(t => t.ProviderId == providerId).OrderBy(t => t.FileName).ToList();
                ViewBag.ProviderId = providerId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForCommandFile(providerId);
                return View(commandFiles);
            }
        }

        public ActionResult Edit(int commandFileId = 0,int providerId=0)
        {
            CommandFile commandFile = null;

            using (var ae = new AvtoritetEntities())
            {
                commandFile = ae.CommandFile.FirstOrDefault(t => t.CommandFileId == commandFileId) ?? new CommandFile();

                ViewBag.ProviderId = providerId;
                return View(commandFile);
            }
        }
        [HttpPost]
        public ActionResult Edit(int commandFileId, int ProviderId, FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    CommandFile newCommandFile = null;
                    if (commandFileId == 0)
                    {
                        newCommandFile = new CommandFile
                        {
                            FileName = collection["filename"],
                            ProviderId = ProviderId,
                            FileContent = collection["filecontent"]
                        };
                        ae.CommandFile.Add(newCommandFile);
                    }
                    else
                    {
                        newCommandFile = ae.CommandFile.FirstOrDefault(t => t.CommandFileId == commandFileId);
                        if (newCommandFile != null)
                        {
                            newCommandFile.FileName = collection["FileName"];
                            newCommandFile.FileContent = collection["FileContent"];
                        }
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index", new { ProviderId = ProviderId });
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id, int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var commandFile = ae.CommandFile.FirstOrDefault(t => t.CommandFileId == id);
                if (commandFile != null)
                {
                    ae.CommandFile.Remove(commandFile);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new { providerId = providerId });
            }
        }

        public ActionResult GetCommandFiles(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var commandFiles = ae.CommandFile.Where(t => t.ProviderId == providerId).Select(t => new
                {
                    commandFileId = t.CommandFileId,
                    providerId = t.ProviderId,
                    filename=t.FileName
                }).ToList();

                return Json(commandFiles, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
