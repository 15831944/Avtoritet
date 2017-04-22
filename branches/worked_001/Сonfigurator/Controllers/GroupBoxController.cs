using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Сonfigurator.DataContext;
using Сonfigurator.Helper;

namespace Сonfigurator.Controllers
{
    public class GroupBoxController : Controller
    {
        // GET: GroupBox
        public ActionResult Index(int groupId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupboxs = ae.GroupBox.Where(t => t.GroupId == groupId).ToList();
                ViewBag.GroupId = groupId;
                ViewBag.BradCrumb = BradCrumb.CreatePathForGroupBox(groupId);
                return View(groupboxs);
            }
        }

        // GET: GroupBox/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GroupBox/Edit/5
        public ActionResult Edit(int groupboxId = 0, int groupId = 0)
        {
            GroupBox groupbox = null;
            using (var ae = new AvtoritetEntities())
            {
                groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == groupboxId) ?? new GroupBox();
                ViewBag.GroupId = groupId;
                return View(groupbox);
            }
        }

        // POST: Group/Edit/5
        [HttpPost]
        public ActionResult Edit(int groupBoxId, int groupId, bool enable,bool VisibleBorder,FormCollection collection)
        {
            try
            {
                using (var ae = new AvtoritetEntities())
                {
                    GroupBox newGroupBox = null;
                    if (groupBoxId == 0)
                    {
                        newGroupBox = new GroupBox
                        {
                            GroupId = groupId,
                            Title = collection["Title"],
                            Enable = enable,
                            Top = !string.IsNullOrEmpty(collection["Top"]) ? int.Parse(collection["Top"]) : 0,
                            Left = !string.IsNullOrEmpty(collection["Left"]) ? int.Parse(collection["Left"]) : 0,
                            Height = !string.IsNullOrEmpty(collection["Height"]) ? int.Parse(collection["Height"]) : 0,
                            Width = !string.IsNullOrEmpty(collection["Width"]) ? int.Parse(collection["Width"]) : 0,
                            VisibleBorder = VisibleBorder,
                            ButtonHeight =
                                !string.IsNullOrEmpty(collection["ButtonHeight"])
                                    ? int.Parse(collection["ButtonHeight"])
                                    : 0,
                            ButtonWidth =
                                !string.IsNullOrEmpty(collection["ButtonWidth"])
                                    ? int.Parse(collection["ButtonWidth"])
                                    : 0,
                            ButtonHorizontalPadding =
                                !string.IsNullOrEmpty(collection["ButtonHorizontalPadding"])
                                    ? int.Parse(collection["ButtonHorizontalPadding"])
                                    : 0,
                            ButtonVerticalPadding =
                                !string.IsNullOrEmpty(collection["ButtonVerticalPadding"])
                                    ? int.Parse(collection["ButtonVerticalPadding"])
                                    : 0,
                            ButtonBetweenVerticalPadding =
                                !string.IsNullOrEmpty(collection["ButtonBetweenVerticalPadding"])
                                    ? int.Parse(collection["ButtonBetweenVerticalPadding"])
                                    : 0,
                            ButtonBetweenHorizontalPadding = 
                                !string.IsNullOrEmpty(collection["ButtonBetweenHorizontalPadding"])
                                    ? int.Parse(collection["ButtonBetweenHorizontalPadding"])
                                    : 0
                        };
                        ae.GroupBox.Add(newGroupBox);
                    }
                    else
                    {
                        newGroupBox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == groupBoxId);
                        if (newGroupBox != null)
                        {
                            newGroupBox.GroupId = groupId;
                            newGroupBox.Title = collection["Title"];
                            newGroupBox.Enable = enable;
                            newGroupBox.VisibleBorder = VisibleBorder;
                            newGroupBox.Top = !string.IsNullOrEmpty(collection["Top"])
                                ? int.Parse(collection["Top"])
                                : 0;
                            newGroupBox.Left = !string.IsNullOrEmpty(collection["Left"])
                                ? int.Parse(collection["Left"])
                                : 0;
                            newGroupBox.Height = !string.IsNullOrEmpty(collection["Height"])
                                ? int.Parse(collection["Height"])
                                : 0;
                            newGroupBox.Width = !string.IsNullOrEmpty(collection["Width"])
                                ? int.Parse(collection["Width"])
                                : 0;
                            newGroupBox.ButtonHeight = !string.IsNullOrEmpty(collection["ButtonHeight"])
                                ? int.Parse(collection["ButtonHeight"])
                                : 0;
                            newGroupBox.ButtonWidth = !string.IsNullOrEmpty(collection["ButtonWidth"])
                                ? int.Parse(collection["ButtonWidth"])
                                : 0;
                            newGroupBox.ButtonHorizontalPadding =
                                !string.IsNullOrEmpty(collection["ButtonHorizontalPadding"])
                                    ? int.Parse(collection["ButtonHorizontalPadding"])
                                    : 0;
                            newGroupBox.ButtonVerticalPadding =
                                !string.IsNullOrEmpty(collection["ButtonVerticalPadding"])
                                    ? int.Parse(collection["ButtonVerticalPadding"])
                                    : 0;
                            newGroupBox.ButtonBetweenVerticalPadding =
                                !string.IsNullOrEmpty(collection["ButtonBetweenVerticalPadding"])
                                    ? int.Parse(collection["ButtonBetweenVerticalPadding"])
                                    : 0;
                            newGroupBox.ButtonBetweenHorizontalPadding =
                                !string.IsNullOrEmpty(collection["ButtonBetweenHorizontalPadding"])
                                    ? int.Parse(collection["ButtonBetweenHorizontalPadding"])
                                    : 0;
                        }
                    }

                    ae.SaveChanges();

                    return RedirectToAction("Index", new {groupId});
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Group/Delete/5
        public ActionResult Delete(int id)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == id);
                if (groupbox != null)
                {
                    ae.GroupBox.Remove(groupbox);
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", new {groupbox.GroupId});
            }
        }

        public ActionResult GetGroupBoxs(int groupId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupboxs = ae.GroupBox.Where(t => t.GroupId == groupId).Select(t => new
                {
                    groupboxId = t.GroupBoxId,
                    name = t.Title,
                    top = t.Top,
                    left = t.Left,
                    width = t.Width,
                    height = t.Height
                }).ToList();

                return Json(groupboxs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetGroupBoxCoord(string groupboxCoord)
        {
            var b = JsonConvert.DeserializeObject<List<GroupBoxCoord>>(groupboxCoord);
            using (var ae = new AvtoritetEntities())
            {
                foreach (var coord in b)
                {
                    var groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == coord.groupboxId);
                    if (groupbox != null)
                    {
                        groupbox.Top = coord.t;
                        groupbox.Left = coord.l;
                        groupbox.Width = coord.w;
                        groupbox.Height = coord.h;
                    }
                }
                ae.SaveChanges();
            }
            return Json("Ok");
        }

        public ActionResult LayOut(int groupboxid, int groupId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == groupboxid);
                ViewBag.Width = groupbox.Width;
                ViewBag.Height = groupbox.Height;
                ViewBag.GroupId = groupId;
                return View(groupboxid);
            }
        }

        public ActionResult ButtonAutoLayout(int groupboxid)
        {
            using (var ae = new AvtoritetEntities())
            {
                //buttons
                var buttons = ae.Brand.Where(t => t.GroupBoxId == groupboxid && t.Enable).OrderBy(t => t.Order).ToList();
                var groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == groupboxid);
                var groupboxCount = ae.GroupBox.Count(t => t.GroupId == groupbox.GroupId);

                if (buttons.Count > 0)
                {
                    var maxWidth =(groupboxCount == 1)?ae.Group.Max(t => t.Width):groupbox.Width;
                    var maxHeight = (groupboxCount == 1)?ae.Group.Max(t => t.Height):groupbox.Height;

                    var rowCount =
                        Math.Floor(
                            (decimal)
                                ((maxHeight - 2*groupbox.ButtonVerticalPadding)/
                                 (groupbox.ButtonHeight + groupbox.ButtonBetweenVerticalPadding)));
                    if (rowCount == 0)
                        rowCount = 1;
                    var colCount =(int) Math.Ceiling(buttons.Count/rowCount);
                    if (colCount == 0)
                        colCount = 1;

                    var col = 1;
                    var row = 1;


                    for (var i = 0; i < buttons.Count; i++)
                    {
                        if (row == 1)
                        {
                            buttons[i].Top = groupbox.ButtonVerticalPadding;

                            buttons[i].Left = groupbox.ButtonHorizontalPadding +
                                              (col - 1)*(groupbox.ButtonWidth + groupbox.ButtonBetweenHorizontalPadding);

                            buttons[i].Width = groupbox.ButtonWidth;
                            buttons[i].Height = groupbox.ButtonHeight;
                        }
                        else
                        {
                            buttons[i].Top = buttons[i - 1].Top + groupbox.ButtonHeight +
                                             groupbox.ButtonBetweenVerticalPadding - 1;

                            buttons[i].Left = groupbox.ButtonHorizontalPadding +
                                              (col - 1)*(groupbox.ButtonWidth + groupbox.ButtonBetweenHorizontalPadding);

                            buttons[i].Width = groupbox.ButtonWidth;
                            buttons[i].Height = groupbox.ButtonHeight;
                        }
                        row++;
                        if (row > rowCount)
                        {
                            col++;
                            row = 1;
                        }
                    }
                    if (groupboxCount == 1)
                    {
                        groupbox.Width = maxWidth;
                        groupbox.Height = maxHeight;
                    }
                    else
                    {
                        if (col > 1)
                        {
                            groupbox.Height = maxHeight;
                        }
                        else
                        {
                            groupbox.Height = buttons[buttons.Count - 1].Top + groupbox.ButtonHeight +
                                              groupbox.ButtonVerticalPadding + buttons.Count * 1 + 10;
                        }
                        groupbox.Width = (2*groupbox.ButtonHorizontalPadding +
                                                 colCount * (groupbox.ButtonWidth + groupbox.ButtonBetweenHorizontalPadding));
                    }
                    ae.SaveChanges();
                }
                return RedirectToAction("Index", "GroupBox", new {groupId = groupbox.GroupId});
            }
        }
    }

    public class GroupBoxCoord
    {
        [JsonProperty("groupboxId")]
        public int groupboxId { get; set; }

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