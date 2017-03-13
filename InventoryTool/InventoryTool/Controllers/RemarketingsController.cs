using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryTool.Models;
using PagedList;

namespace InventoryTool.Controllers
{
    public class RemarketingsController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: Remarketings
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FleetSortParm = String.IsNullOrEmpty(sortOrder) ? "Fleet Number" : "";
            ViewBag.LogSortParm = String.IsNullOrEmpty(sortOrder) ? "Log Number" : "";
            ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status" : "";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var Remarketings = from s in db.Remarketings
                               select s;

            if (!String.IsNullOrEmpty(searchString))
                Remarketings = Remarketings.Where(s => s.LogNumber.ToString().Contains(searchString) || s.FleetNumber.ToString().Contains(searchString) || s.Status.ToString().Contains(searchString));

            switch (sortOrder)
            {
                case "Log Number":
                    Remarketings = Remarketings.OrderByDescending(s => s.LogNumber);
                    break;
                case "Fleet Number":
                    Remarketings = Remarketings.OrderBy(s => s.FleetNumber);
                    break;
                case "Status":
                    Remarketings = Remarketings.OrderBy(s => s.Status);
                    break;
                default:
                    Remarketings = Remarketings.OrderByDescending(s => s.ID);
                    break;
            }

            int pageSize = 200;
            int pageNumber = (page ?? 1);
            return View(Remarketings.ToPagedList(pageNumber, pageSize));

        }

        // GET: Remarketings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remarketing remarketing = db.Remarketings.Find(id);
            if (remarketing == null)
            {
                return HttpNotFound();
            }
            return View(remarketing);
        }

        // GET: Remarketings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Remarketings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,Status")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                db.Remarketings.Add(remarketing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(remarketing);
        }

        // GET: Remarketings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remarketing remarketing = db.Remarketings.Find(id);
            if (remarketing == null)
            {
                return HttpNotFound();
            }
            return View(remarketing);
        }

        // POST: Remarketings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,Status")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remarketing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(remarketing);
        }

        // GET: Remarketings/OffEdit/5
        public ActionResult OffEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remarketing remarketing = db.Remarketings.Find(id);
            if (remarketing == null)
            {
                return HttpNotFound();
            }
            return View(remarketing);
        }

        // POST: Remarketings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OffEdit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,Status")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remarketing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(remarketing);
        }

        // GET: Remarketings/OffEdit/5
        public ActionResult SaleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remarketing remarketing = db.Remarketings.Find(id);
            if (remarketing == null)
            {
                return HttpNotFound();
            }
            return View(remarketing);
        }

        // POST: Remarketings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaleEdit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,Status,SoldDate,OutletCode,Outletname,SaleValue")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remarketing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(remarketing);
        }

        // GET: Remarketings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remarketing remarketing = db.Remarketings.Find(id);
            if (remarketing == null)
            {
                return HttpNotFound();
            }
            return View(remarketing);
        }

        // POST: Remarketings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Remarketing remarketing = db.Remarketings.Find(id);
            db.Remarketings.Remove(remarketing);
            db.SaveChanges();
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

