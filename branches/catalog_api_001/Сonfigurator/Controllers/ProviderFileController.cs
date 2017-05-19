using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class ProviderFileController : Controller
    {
        public ActionResult Index(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var providerFiles = ae.ProviderFile.Where(t => t.ProviderId == providerId).OrderBy(t => t.FileName).ToList();
                ViewBag.providerId = providerId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForProviderFile(providerId);
                return View(providerFiles);
            }
        }
        [HttpPost]
        public ActionResult Upload(int providerId,FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    ProviderFile providerFile=new ProviderFile();
                    UploadProviderFile(providerFile,collection);
                    providerFile.ProviderId = providerId;
                    ae.ProviderFile.Add(providerFile);
                    ae.SaveChanges();

                    return RedirectToAction("Index", new { providerId = providerId });
                }
            }
            catch
            {
                return View("Index");
            }
        }

     
        private void UploadProviderFile(ProviderFile providerFile, FormCollection collection)
        {
            foreach (string upload in Request.Files)
            {
                var mimeType = Request.Files[upload].ContentType;
                var fileStream = Request.Files[upload].InputStream;
                var fileName = Path.GetFileName(Request.Files[upload].FileName);
                if (string.IsNullOrEmpty(fileName))
                    continue;
                var fileLength = Request.Files[upload].ContentLength;
                var fileData = new byte[fileLength];
                var size = fileStream.Read(fileData, 0, fileLength);

                if (upload == "upload")
                {
                    providerFile.FileContent = fileData;
                    providerFile.FileName = fileName;
                    providerFile.FileExt = Path.GetExtension(fileName);
                    providerFile.FileSize = fileLength;
                }
            }
        }


        // GET: Brand/Delete/5
        public ActionResult Delete(int id, int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var providerFile = ae.ProviderFile.FirstOrDefault(t => t.ProviderFileId == id);
                if (providerFile != null)
                {
                    ae.ProviderFile.Remove(providerFile);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new { providerId = providerId });
            }
        }
    }
}