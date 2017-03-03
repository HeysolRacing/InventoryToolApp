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


namespace InventoryTool.Controllers
{
    public class FleetsController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        private Fleet Calculation(Fleet fleet)
        {
            // Penalty Calculation
            if (fleet.Inservice_date != null)
            {
                CalculateDate PenaltyDD = new CalculateDate(DateTime.Parse(fleet.Inservice_date.ToString()), DateTime.Now);
                fleet.Penalty = "Penalty - " + PenaltyDD.Days.ToString();
            }
            else
            {
                fleet.Penalty = "Penalty - 0";
            }

            // Lease Month Calculation
            //if (fleet.Insurance > 0 && fleet.Amort_Term > 0)
            //{
            //    fleet.Penalty = "Penalty - " + PenaltyDD.Days.ToString();
            //}
            //else
            //{
            //    fleet.Penalty = "Penalty - 0";
            //}

            // Lease Month Calculation
            if (fleet.Insurance > 0 && fleet.Amort_Term > 0)
            {
                CalculateDate PenaltyDD = new CalculateDate(DateTime.Parse(fleet.Inservice_date.ToString()), DateTime.Now);
                fleet.Penalty = "Penalty - " + PenaltyDD.Days.ToString();
            }
            else
            {
                fleet.Penalty = "Penalty - 0";
            }

            return fleet;
        }

        [Authorize(Roles = "InventoryView")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LogNumber" : "";
            ViewBag.DateSortParm = sortOrder == "UnitNumber" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var fleets = from s in db.Fleets
                         select s;

            if (!String.IsNullOrEmpty(searchString))
                fleets = fleets.Where(s => s.LogNumber.ToString().Contains(searchString) || s.FleetNumber.ToString().Contains(searchString));
            else
                fleets = fleets.Take(200);

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

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(fleets.ToPagedList(pageNumber, pageSize));
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
                fleet.CreatedBy = Environment.UserName;
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
                fleet.CreatedBy = Environment.UserName;
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
