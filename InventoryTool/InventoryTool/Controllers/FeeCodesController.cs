using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryTool.Models;
using PagedList;

namespace InventoryTool.Controllers
{
    public class FeeCodesController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: FeeCodes
        [Authorize(Roles = "FeeCodesView")]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.UnitSortParm = String.IsNullOrEmpty(sortOrder) ? "UNIT" : "";
            ViewBag.FeeSortParm = String.IsNullOrEmpty(sortOrder) ? "FEE" : "";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "MMYY" : "";


            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var fleets = from s in db.FeeCodes
                         select s;

            if (!String.IsNullOrEmpty(searchString))
                fleets = fleets.Where(s => s.Fee.ToString().Equals(searchString) || s.Fleet.ToString().Equals(searchString) || s.Unit.ToString().Equals(searchString) || s.LogNo.ToString().Equals(searchString));
            else
                fleets = fleets.Take(200);

            switch (sortOrder)
            {
                case "FEE":
                    fleets = fleets.OrderByDescending(s => s.Fee);
                    break;
                case "UNIT":
                    fleets = fleets.OrderBy(s => s.Unit);
                    break;
                case "MMYY":
                    fleets = fleets.OrderByDescending(s => s.MMYY);
                    break;
                default:
                    fleets = fleets.OrderBy(s => s.Fleet);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(fleets.ToPagedList(pageNumber, pageSize));
        }

        // GET: FeeCodes/Details/5
        [Authorize(Roles = "FeeCodesView")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeCode feeCode = await db.FeeCodes.FindAsync(id);
            if (feeCode == null)
            {
                return HttpNotFound();
            }
            return View(feeCode);
        }

        // GET: FeeCodes/Create
        [Authorize(Roles = "FeeCodesCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FeeCode_Id,Fleet,Unit,LogNo,CapCost,BookValue,Term,Lpis,OnRdDat,OfRdDat,Scontr,InsPremium,ResidualAmt,Fee,Desc,MMYY,Start,Stop,Amt,Method,Rate,BL,AC,Createdby,Created")] FeeCode feeCode)
        {
            if (ModelState.IsValid)
            {
                feeCode.Createdby = Environment.UserName;
                feeCode.Created = DateTime.Now.ToString();
                db.FeeCodes.Add(feeCode);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feeCode);
        }

        // GET: FeeCodes/Edit/5
        [Authorize(Roles = "FeeCodesEdit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeCode feeCode = await db.FeeCodes.FindAsync(id);
            if (feeCode == null)
            {
                return HttpNotFound();
            }
            return View(feeCode);
        }

        // POST: FeeCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FeeCode_Id,Fleet,Unit,LogNo,CapCost,BookValue,Term,Lpis,OnRdDat,OfRdDat,Scontr,InsPremium,ResidualAmt,Fee,Desc,MMYY,Start,Stop,Amt,Method,Rate,BL,AC,Createdby,Created")] FeeCode feeCode)
        {
            if (ModelState.IsValid)
            {
                feeCode.Createdby = Environment.UserName;
                feeCode.Created = DateTime.Now.ToString();
                db.Entry(feeCode).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feeCode);
        }

        // GET: FeeCodes/Delete/5
        [Authorize(Roles = "FeeCodesDelete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeCode feeCode = await db.FeeCodes.FindAsync(id);
            if (feeCode == null)
            {
                return HttpNotFound();
            }
            return View(feeCode);
        }

        // POST: FeeCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeeCode feeCode = await db.FeeCodes.FindAsync(id);
            db.FeeCodes.Remove(feeCode);
            await db.SaveChangesAsync();
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
