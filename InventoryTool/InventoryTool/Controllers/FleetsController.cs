using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using InventoryTool.Models;
using PagedList;
using System.Security.Claims;

namespace InventoryTool.Controllers
{
    public class FleetsController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        [Authorize(Roles = "InventoryView")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, string currentLogNo, string searchLogNo)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LogNumber" : "";
            ViewBag.DateSortParm = sortOrder == "UnitNumber" ? "date_desc" : "Date";

            if ((searchString != null) || (searchLogNo != null))
                page = 1;
            else
            {
                searchString = currentFilter;
                searchLogNo = currentLogNo;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentLogNo = searchLogNo;

            var fleets = from s in db.Fleets.Include(d => d.Driven)
                         select s;

            if (!String.IsNullOrEmpty(searchString))
                fleets = fleets.Where(s => s.FleetNumber.ToString().Contains(searchString));

            if (!String.IsNullOrEmpty(searchLogNo))
                fleets = fleets.Where(s => s.LogNumber.ToString().Contains(searchLogNo));
            //else
            //    fleets = fleets.Take(200);

            switch (sortOrder)
            {
                case "LogNumber":
                    fleets = fleets.OrderByDescending(s => s.LogNumber);
                    break;
                case "UnitNumber":
                    fleets = fleets.OrderBy(s => s.UnitNumber);
                    break;
                case "date_desc":
                    fleets = fleets.OrderByDescending(s => s.Inservice_process);
                    break;
                default:
                    fleets = fleets.OrderBy(s => s.Inservice_date);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(fleets.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "PhantomView")]
        public ViewResult Phantom(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VINSortParm = String.IsNullOrEmpty(sortOrder) ? "Vin Number" : "";
            ViewBag.FleetSortParm = String.IsNullOrEmpty(sortOrder) ? "Fleet Number" : "";
            ViewBag.UnitSortParm = String.IsNullOrEmpty(sortOrder) ? "Unit Number" : "";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var fleets = from s in db.Fleets
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            { fleets = fleets.Where(s => s.VinNumber.ToString().Contains(searchString) || s.FleetNumber.ToString().Contains(searchString) || s.UnitNumber.ToString().Contains(searchString)
            &&((s.Offroad_date == null && s.ContractType.Contains("N5"))));
            }
            else
            {
                fleets = fleets.Where(s => s.Offroad_date == null && s.ContractType.ToString().Contains("N5"));
            }

            switch (sortOrder)
            {
                case "Vin Number":
                    fleets = fleets.OrderByDescending(s => s.VinNumber);
                    break;
                case "Fleet Number":
                    fleets = fleets.OrderBy(s => s.FleetNumber);
                    break;
                case "Unit Number":
                    fleets = fleets.OrderBy(s => s.FleetNumber);
                    break;
                default:
                    fleets = fleets.OrderBy(s => s.Inservice_date);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(fleets.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "PhantomCreate")]
        public ActionResult Select(int? id)
        {
            Fleet fleet = db.Fleets.Find(id);
            CR cr = new CR();
            cr.VINnumber = fleet.VinNumber;
            cr.FleetNumber = fleet.FleetNumber;
            cr.UnitNumber = fleet.UnitNumber;
            cr.Status = "none";
            cr.Clientname = fleet.Level_2;
            var userIdValue = Environment.UserName;


            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }

            cr.CreatedBy = userIdValue;
            cr.Servicedate = DateTime.Now;
            cr.Invoicedate = DateTime.Now;
            cr.Paymentdate = DateTime.Now;
            db.CRs.Add(cr);
            db.SaveChanges();
            var crid = cr.crID;
            return RedirectToAction("Create","CRs", new { id = crid });
        }

        [Authorize(Roles = "InventoryView")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return HttpNotFound();
            }
            return View(fleet);
        }

        [Authorize(Roles = "InventoryCreate")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "InventoryEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return HttpNotFound();
            }
            return View(fleet);
        }

        [Authorize(Roles = "InventoryDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return HttpNotFound();
            }
            return View(fleet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FleetID,LogNumber,CorpCode,FleetNumber,UnitNumber,VinNumber,ContractType,Make,ModelCar,ModelYear,BookValue,CapCost,Inservice_date,Inservice_process,Original_Inservice,Original_Process,Offroad_date,Offroad_process,Sold_date,Sold_process,FleetCancelUnit,Amort_Term,Leased_Months_Billed,End_date,ScontrNumber,Amort,LicenseNumber,State,Roe,DealerName,Insurance,Secdep,DepartmentCode,Residual_Amount,Level_1,Level_2,Level_3,Level_4,Level_5,Level_6,TTL,OutletCode,OutletName,Created,CreatedBy")] Fleet fleet)
        {
            if (ModelState.IsValid)
            {
                fleet.Created = DateTime.Now;
                var userIdValue = Environment.UserName;


                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.Name);

                    if (userIdClaim != null)
                    {
                        userIdValue = userIdClaim.Value;
                    }
                }
                fleet.CreatedBy = userIdValue;
                db.Fleets.Add(fleet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fleet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FleetID,LogNumber,CorpCode,FleetNumber,UnitNumber,VinNumber,ContractType,Make,ModelCar,ModelYear,BookValue,CapCost,Inservice_date,Inservice_process,Original_Inservice,Original_Process,Offroad_date,Offroad_process,Sold_date,Sold_process,FleetCancelUnit,Amort_Term,Leased_Months_Billed,End_date,ScontrNumber,Amort,LicenseNumber,State,Roe,DealerName,Insurance,Secdep,DepartmentCode,Residual_Amount,Level_1,Level_2,Level_3,Level_4,Level_5,Level_6,TTL,OutletCode,OutletName,Created,CreatedBy")] Fleet fleet)
        {
            if (ModelState.IsValid)
            {
                fleet.Created = DateTime.Now;
                var userIdValue = Environment.UserName;


                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.Name);

                    if (userIdClaim != null)
                    {
                        userIdValue = userIdClaim.Value;
                    }
                }
                fleet.CreatedBy = userIdValue;
                db.Entry(fleet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fleet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fleet fleet = db.Fleets.Find(id);
            db.Fleets.Remove(fleet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Fleets.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=FleetAll.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
