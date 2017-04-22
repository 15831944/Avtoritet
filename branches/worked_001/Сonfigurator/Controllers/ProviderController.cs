using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class ProviderController : Controller
    {
        // GET: Provider
        public ActionResult Index(int brandId=0)
        {
            using (var ae = new AvtoritetEntities())
            {

                var providers = ae.Provider.Where(t=>t.BrandId== brandId).OrderBy(t => t.Order).ToList();
                ViewBag.brandId = brandId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForProvider(brandId);
                return View(providers);
            }
        }

        // GET: Provider/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Provider/Edit/5
        public ActionResult Edit(int providerId=0,int brandId=0)
        {
            Provider provider = null;
            using (var ae = new AvtoritetEntities())
            {
                provider = ae.Provider.FirstOrDefault(t => t.ProviderId == providerId) ?? new Provider();
                ViewBag.BrandId = brandId;
                return View(provider);
            }
        }

        // POST: Provider/Edit/5
        [HttpPost]
        public ActionResult Edit(int providerId,int brandId,bool enable, FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    Provider provider = null;
                    if (providerId == 0)
                    {
                        provider = new Provider
                        {
                            Uri = collection["Uri"],
                            Title = collection["Title"],
                            //IconPath = collection["IconPath"],
                            BrandId = brandId,
                            commandcontent = collection["commandcontent"],
                            Enable = enable,
                            Order = !string.IsNullOrEmpty(collection["order"])?int.Parse(collection["order"]):0
                        };
                        UploadProviderFile(provider, collection);
                        ae.Provider.Add(provider);
                    }
                    else
                    {
                        provider = ae.Provider.FirstOrDefault(t => t.ProviderId == providerId);
                        if (provider != null)
                        {
                            provider.Title = collection["Title"];
                            //provider.IconPath = collection["IconPath"];
                            provider.Uri = collection["Uri"];
                            provider.commandcontent = collection["commandcontent"];
                            provider.Enable = enable;
                            provider.Order = !string.IsNullOrEmpty(collection["order"])
                                ? int.Parse(collection["order"])
                                : 0;
                        }
                        UploadProviderFile(provider, collection);
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index", new {BrandId = brandId});
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ProviderImage(int id)
        {
            using (var ae = new AvtoritetEntities())
            {
                var image = ae.Provider.FirstOrDefault(t => t.ProviderId == id);
                if (image!=null && image.IconImg != null)
                    return File(image.IconImg, "iamge/" + (!string.IsNullOrEmpty(image.IconImgExt)?image.IconImgExt.Substring(1):string.Empty));
                else
                    return null;
            }
        }
        private void UploadProviderFile(Provider provider, FormCollection collection)
        {
            foreach (string upload in Request.Files)
            {
                string mimeType = Request.Files[upload].ContentType;
                Stream fileStream = Request.Files[upload].InputStream;
                string fileName = Path.GetFileName(Request.Files[upload].FileName);
                if (string.IsNullOrEmpty(fileName))
                    continue;
                int fileLength = Request.Files[upload].ContentLength;
                byte[] fileData = new byte[fileLength];
                int size=fileStream.Read(fileData, 0, fileLength);

                provider.IconImg = fileData;
                provider.IconPath = fileName;
                provider.IconImgSize = fileLength;
                provider.IconImgExt = Path.GetExtension(fileName);
            }
        }
        // GET: Provider/Delete/5
        public ActionResult Delete(int id,int brandId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var provider = ae.Provider.FirstOrDefault(t => t.ProviderId == id);
                if (provider != null)
                {
                    ae.Provider.Remove(provider);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new { brandId=brandId });
            }
        }
    }
}
