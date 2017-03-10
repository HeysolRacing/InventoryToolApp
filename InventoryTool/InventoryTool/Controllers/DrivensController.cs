using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryTool.Models;

namespace InventoryTool.Controllers
{
    public class DrivensController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: Drivens
        public ActionResult Index()
        {
            return View(db.Drivens.ToList());
        }

        // GET: Drivens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driven driven = db.Drivens.Find(id);
            if (driven == null)
            {
                return HttpNotFound();
            }
            return View(driven);
        }

        // GET: Drivens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drivens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CostumerReference,ClientUnit,VRN,DriverName,DriverLastName,Address1,Address2,City,State,ZIP")] Driven driven)
        {
            if (ModelState.IsValid)
            {
                db.Drivens.Add(driven);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driven);
        }

        // GET: Drivens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driven driven = db.Drivens.Find(id);
            if (driven == null)
            {
                return HttpNotFound();
            }
            return View(driven);
        }

        // POST: Drivens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CostumerReference,ClientUnit,VRN,DriverName,DriverLastName,Address1,Address2,City,State,ZIP")] Driven driven)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driven).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driven);
        }

        // GET: Drivens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driven driven = db.Drivens.Find(id);
            if (driven == null)
            {
                return HttpNotFound();
            }
            return View(driven);
        }

        // POST: Drivens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Driven driven = db.Drivens.Find(id);
            db.Drivens.Remove(driven);
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
