using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        public ActionResult Index(int groupboxId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var brands = ae.Brand.Where(t => t.GroupBoxId == groupboxId).OrderBy(t => t.NameAndFolder).ToList();
                ViewBag.GroupBoxId = groupboxId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForBrand(groupboxId);
                return View(brands);
            }
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(int brandid = 0, int groupboxId = 0)
        {
            Brand brand = null;
          
            using (var ae = new AvtoritetEntities())
            {
                brand = ae.Brand.FirstOrDefault(t => t.BrandId == brandid) ?? new Brand();

                ViewBag.GroupBoxId = groupboxId;
                ViewBag.StyleList = GetButtonStyles(brand.ButtonStyle);

                return View(brand);
            }
        }
        private IEnumerable<SelectListItem> GetButtonStyles(string selectValue)
        {
            var style1 = new SelectListItem
            {
                Text = "Стиль с иконками",
                Value = "ButtonStyle1",
                Selected = selectValue== "ButtonStyle1"
            };
            var style2 = new SelectListItem
            {
                Text = "Стиль без иконок",
                Value = "ButtonStyle2",
                Selected = selectValue == "ButtonStyle2"
            };
            var style3 = new SelectListItem
            {
                Text = "Стиль c иконкой без подложки",
                Value = "ButtonStyle3",
                Selected = selectValue == "ButtonStyle3"
            };
            List<SelectListItem> styleList = new List<SelectListItem>();
            styleList.Add(style1);
            styleList.Add(style2);
            styleList.Add(style3);
            return new SelectList(styleList, "Value", "Text");
        }
        // POST: Brand/Edit/5
        [HttpPost]
        public ActionResult Edit(int brandId, int groupboxId, bool enable,bool menuwindow, FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    Brand newBrand = null;
                    if (brandId == 0)
                    {
                        newBrand = new Brand
                        {
                            NameAndFolder = collection["NameAndFolder"],
                            //IconPath = collection["IconPath"],
                            //IconPath2 = collection["IconPath"],
                            GroupBoxId = groupboxId,
                            Enable = enable,
                            MenuWindow = menuwindow,
                            Top = !string.IsNullOrEmpty(collection["Top"]) ? int.Parse(collection["Top"]) : 0,
                            Left = !string.IsNullOrEmpty(collection["Left"]) ? int.Parse(collection["Left"]) : 0,
                            Width = !string.IsNullOrEmpty(collection["Width"]) ? int.Parse(collection["Width"]) : 0,
                            Height = !string.IsNullOrEmpty(collection["Height"]) ? int.Parse(collection["Height"]) : 0,
                            Order = !string.IsNullOrEmpty(collection["Order"]) ? int.Parse(collection["Order"]) : 0,
                            ButtonStyle = !string.IsNullOrEmpty(collection["ButtonStyle"]) ? collection["ButtonStyle"] :string.Empty
                        };
                        UploadBrandFile(newBrand, collection);
                        ae.Brand.Add(newBrand);
                    }
                    else
                    {
                        newBrand = ae.Brand.FirstOrDefault(t => t.BrandId == brandId);
                        if (newBrand != null)
                        {
                            newBrand.NameAndFolder = collection["NameAndFolder"];
                            //newBrand.IconPath = collection["IconPath"];
                            //newBrand.IconPath2 = collection["IconPath2"];
                            newBrand.Enable = enable;
                            newBrand.MenuWindow = menuwindow;
                            newBrand.Top = !string.IsNullOrEmpty(collection["Top"]) ? int.Parse(collection["Top"]) : 0;
                            newBrand.Left = !string.IsNullOrEmpty(collection["Left"])
                                ? int.Parse(collection["Left"])
                                : 0;
                            newBrand.Width = !string.IsNullOrEmpty(collection["Width"])
                                ? int.Parse(collection["Width"])
                                : 0;
                            newBrand.Height = !string.IsNullOrEmpty(collection["Height"])
                                ? int.Parse(collection["Height"])
                                : 0;
                            newBrand.Order = !string.IsNullOrEmpty(collection["Order"])
                                ? int.Parse(collection["Order"])
                                : 0;
                            newBrand.ButtonStyle = !string.IsNullOrEmpty(collection["ButtonStyle"])
                                ? collection["ButtonStyle"]
                                : string.Empty;


                        }
                        UploadBrandFile(newBrand, collection);
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index", new {GroupBoxId = groupboxId});
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BrandImage(int id)
        {
            using (var ae = new AvtoritetEntities())
            {
                var image = ae.Brand.FirstOrDefault(t => t.BrandId == id);
                if (image != null && image.IconPathImg != null)
                    return File(image.IconPathImg,
                        "image/" +
                        (!string.IsNullOrEmpty(image.IconPathImgExt) ? image.IconPathImgExt.Substring(1) : string.Empty));
                return null;
            }
        }

        public ActionResult BrandImage2(int id)
        {
            using (var ae = new AvtoritetEntities())
            {
                var image = ae.Brand.FirstOrDefault(t => t.BrandId == id);
                if (image != null && image.IconPath2Img != null)
                    return File(image.IconPath2Img,
                        "image/" +
                        (!string.IsNullOrEmpty(image.IconPath2ImgExt)
                            ? image.IconPath2ImgExt.Substring(1)
                            : string.Empty));
                return null;
            }
        }

        private void UploadBrandFile(Brand brand, FormCollection collection)
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
                    brand.IconPathImg = fileData;
                    brand.IconPath = fileName;
                    brand.IconPathImgExt = Path.GetExtension(fileName);
                    brand.IconPathImgSize = fileLength;
                }
                if (upload == "upload2")
                {
                    brand.IconPath2Img = fileData;
                    brand.IconPath2 = fileName;
                    brand.IconPath2ImgExt = Path.GetExtension(fileName);
                    brand.IconPath2ImgSize = fileLength;
                }
            }
        }


        // GET: Brand/Delete/5
        public ActionResult Delete(int id, int groupboxId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var brand = ae.Brand.FirstOrDefault(t => t.BrandId == id);
                if (brand != null)
                {
                    ae.Brand.Remove(brand);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new {groupboxId = groupboxId});
            }
        }

        public ActionResult GetBrands(int groupboxId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var brands = ae.Brand.Where(t => t.GroupBoxId == groupboxId).Select(t => new
                {
                    brandId=t.BrandId,
                    name = t.NameAndFolder,
                    top = t.Top,
                    left = t.Left,
                    width=t.Width,
                    height=t.Height
                }).ToList();

                return Json(brands, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetBrandCoord(string brandCoord)
        {
            List<BrandCoord> b = JsonConvert.DeserializeObject<List<BrandCoord>>(brandCoord);
            using (var ae = new AvtoritetEntities())
            {
                foreach (var coord in b)
                {
                    var brand = ae.Brand.FirstOrDefault(t => t.BrandId == coord.brandId);
                    if (brand != null)
                    {
                        brand.Top = coord.t;
                        brand.Left = coord.l;
                        brand.Width = coord.w;
                        brand.Height = coord.h;
                    }
                }
                ae.SaveChanges();
            }
            return Json("Ok");
        }
    }
    public class BrandCoord
    {
        [JsonProperty("brandId")]
        public int brandId { get; set; }
        [JsonProperty("w")]
        public int w { get; set; }
        [JsonProperty("h")]
        public int h { get; set; }
        [JsonProperty("l")]
        public int l { get; set; }
        [JsonProperty("t")]
        public int t { get; set; }

    }
}