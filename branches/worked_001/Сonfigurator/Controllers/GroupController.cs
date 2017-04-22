using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Сonfigurator.DataContext;

namespace Сonfigurator.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            using (var ae=new AvtoritetEntities())
            {
                var groups = ae.Group.OrderBy(t=>t.Order).ToList();
                return View(groups);
            }
        }

        // GET: Group/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int groupId=0)
        {
            Group group = null;
            using (var ae = new AvtoritetEntities())
            {
                group = ae.Group.FirstOrDefault(t => t.GroupId == groupId) ?? new Group();
                return View(group);
            }
        }

        // POST: Group/Edit/5
        [HttpPost]
        public ActionResult Edit(int groupId,bool enable, FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    Group newGroup = null;
                    if (groupId == 0)
                    {
                        newGroup = new Group
                        {
                            Name = collection["Name"],
                            Width = !string.IsNullOrEmpty(collection["Width"]) ? int.Parse(collection["Width"]) : 0,
                            Height = !string.IsNullOrEmpty(collection["Height"]) ? int.Parse(collection["Height"]) : 0,
                            Description = collection["Description"],
                            Enable = enable,
                            Order = !string.IsNullOrEmpty(collection["order"])?int.Parse(collection["order"]):0
                        };
                        ae.Group.Add(newGroup);
                    }
                    else
                    {
                        newGroup = ae.Group.FirstOrDefault(t => t.GroupId == groupId);
                        if (newGroup != null)
                        {
                            newGroup.Name = collection["Name"];
                            newGroup.Width = !string.IsNullOrEmpty(collection["Width"])
                                ? int.Parse(collection["Width"])
                                : 0;
                            newGroup.Height = !string.IsNullOrEmpty(collection["Height"])
                                ? int.Parse(collection["Height"])
                                : 0;
                            newGroup.Description = collection["Description"];
                            newGroup.Enable =enable;
                            newGroup.Order = !string.IsNullOrEmpty(collection["order"])
                                ? int.Parse(collection["order"])
                                : 0;
                        }
                    }

                    ae.SaveChanges();

                    //выравнивание

                    var groups = ae.Group.ToList();
                    foreach (var group in groups)
                    {
                        group.Height = newGroup.Height;
                        group.Width = newGroup.Width;
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Group/Delete/5
        public ActionResult Delete(int id)
        {

            using (var ae = new AvtoritetEntities())
            {
                var group = ae.Group.FirstOrDefault(t => t.GroupId == id);
                if (group != null)
                {
                    ae.Group.Remove(group);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult LayOut(int groupId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupWidth = ae.Group.Max(t => t.Width);
                var groupHeight = ae.Group.Max(t => t.Height);

                ViewBag.Width = groupWidth;
                ViewBag.Height = groupHeight;
                return View(groupId);
            }
        }

        public ActionResult Publish()
        {
            using (var ae = new AvtoritetEntities())
            {
                var settingUpdate=ae.SettingUpdate.FirstOrDefault();
                if (settingUpdate != null)
                {
                    settingUpdate.Update =false;
                    settingUpdate.SettingVersion += 1;
                    ae.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
