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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Globalization;

namespace InventoryTool.Controllers
{
    public class RemarketingsController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: Remarketings
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string fleetString, string unitString, string logString, int? page)
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
            ViewBag.fleetFilter = fleetString;
            ViewBag.logFilter = logString;
            ViewBag.UnitSortParm = unitString;

            var Remarketings = from s in db.Remarketings
                               select s;

            if (!String.IsNullOrEmpty(searchString))
                Remarketings = Remarketings.Where(s => s.Status.ToString().Contains(searchString));
            if (!String.IsNullOrEmpty(fleetString))
                Remarketings = Remarketings.Where(s => s.FleetNumber.ToString().Contains(fleetString));
            if (!String.IsNullOrEmpty(logString))
                Remarketings = Remarketings.Where(s => s.LogNumber.ToString().Contains(logString));
            if (!String.IsNullOrEmpty(unitString))
                Remarketings = Remarketings.Where(s => s.UnitNumber.ToString().Contains(unitString));

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
        public ActionResult Create([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,BankAccount,Status,SoldDate,OutletCode,Outletname,Quote")] Remarketing remarketing)
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
        public ActionResult Edit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,BankAccount,Status,SoldDate,OutletCode,Outletname,Quote")] Remarketing remarketing)
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

        // POST: Remarketings/OffEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OffEdit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,BankAccount,Status,SoldDate,OutletCode,Outletname,Quote")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                remarketing.Quote = "FALSE";
                db.Entry(remarketing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(remarketing);
        }

        // GET: Remarketings/QuoteEdit/5
        public ActionResult QuoteEdit(int? id)
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

        // POST: Remarketings/QuoteEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuoteEdit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,Status,SoldDate,OutletCode,Outletname,Quote")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remarketing).State = EntityState.Modified;
                db.SaveChanges();
                if (!String.IsNullOrEmpty(remarketing.CurrentPeriod.ToString()) && String.IsNullOrEmpty(remarketing.Amortization.ToString()))
                {
                    this.HttpContext.Session["DisplayAmort"] = "The Amount of Amortization in current period doesn't exist";
                }
                else { this.HttpContext.Session["DisplayAmort"] = ""; }
                if (!String.IsNullOrEmpty(remarketing.CurrentPeriod.ToString()) && String.IsNullOrEmpty(remarketing.Interest.ToString()))
                {
                    this.HttpContext.Session["Display"] = "The Amount of Interest in current period doesn't exist";
                }
                else { this.HttpContext.Session["Display"] = ""; }
                return RedirectToAction("QuoteEdit");
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
            var outletcodes = db.OutletCodes.ToList();
            foreach (OutletCode item in outletcodes)
            { item.Outletname = item.Outletcode + "-" + item.Outletname; }
            ViewBag.OutletCodes = outletcodes;

            var bankaccounts = db.BankAccounts.ToList();
            foreach (BankAccounts item in bankaccounts)
            { item.Currency = item.Currency + " - " + item.Account; }
            ViewBag.BankAccounts = bankaccounts;

            return View(remarketing);
        }

        // POST: Remarketings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaleEdit([Bind(Include = "ID,FleetNumber,UnitNumber,LogNumber,Roe,SpotRate,ScontrNumber,OnroadDate,EndDate,Term,OffroadDate,CurrentPeriod,Amortization,Interest,Rent,RemainingMonths,Rate,Penalty,SaleValue,BookValue,GainLoss,ProfitShareAmount,ProfitSharePercentage,ComplementaryRent,CreditNote,PLGainLoss,BankAccount,Status,SoldDate,OutletCode,Outletname,Quote")] Remarketing remarketing)
        {
            if (ModelState.IsValid)
            {
                var outlet = from s in db.OutletCodes
                             where s.Outletcode.Contains(remarketing.OutletCode)
                             select s;
                remarketing.Outletname = outlet.ToList()[0].Outletname;
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

        //POST: Remarketings/ExportData
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            var crs = from a in db.Remarketings
                      select a;
            gv.DataSource = crs.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CrsList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("General");
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

        public ActionResult ApRemarketing()
        {
            var crs = from c in db.Remarketings
                      join f in db.Fleets on c.LogNumber equals f.LogNumber

                      select new
                      {
                          c.FleetNumber,
                          c.UnitNumber,
                          c.LogNumber,
                          c.Penalty,
                          c.GainLoss,
                          c.ProfitShareAmount,
                          c.ComplementaryRent,
                          c.CreditNote,
                          c.Roe,
                          c.SpotRate,
                          c.Status,
                          c.SoldDate,
                          c.BankAccount
                      };
            crs = crs.Where(s => s.Status.ToUpper().Equals("SOLD"));

            DateTime time = DateTime.Now;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            // Return the week of our adjusted day
            int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            List<APRemarketing> aPRemarketing = new List<Models.APRemarketing>();
            foreach (var item in crs)
            {
                APRemarketing ag = new APRemarketing();
                ag.JournalCategory = "?Remarketing";
                ag.JournalSource = "?payables";
                ag.CorpCode = 501;
                ag.CostCentre = "Diana provee info";
                ag.Account = "Diana provee info";
                ag.BankAccount = item.BankAccount;
                ag.PeriodName = "APR-17";
                ag.CRNumber = "Número de transacción";
                ag.DAN = item.LogNumber;
                ag.FleetNumber = item.FleetNumber;
                ag.UnitNumber = item.UnitNumber;
                ag.ChargeCode = "Diana provee info";
                ag.ExternalDocLocator = "Número de factura del activo";
                ag.CorpCodeAP = "N/A";
                ag.ProcessDate = "Fecha de transacción";
                ag.InternalDocumentLocator = "Número de documento contable";
                ag.PartyName = "N/A";
                ag.TransactionDate = Convert.ToDateTime(item.SoldDate);
                ag.EnteredDrSUM = Convert.ToDecimal(item.Penalty) + Convert.ToDecimal(item.ProfitShareAmount);
                ag.EnteredCrSUM = Convert.ToDecimal(item.ComplementaryRent) + Convert.ToDecimal(item.CreditNote);
                ag.NET = ag.EnteredDrSUM - ag.EnteredCrSUM;
                ag.AccountedDrSUM = ag.EnteredDrSUM * item.Roe;
                ag.AccountedCrSUM = ag.EnteredCrSUM * item.Roe;
                ag.CurrencyCode = item.Roe.Equals(0) ? "USD" : "MXN";
                ag.Charge = "N/A";
                ag.TCInvoice = item.Roe.Equals(0) ? item.Roe : 1;
                ag.TCPayment = item.Roe.Equals(0) ? Convert.ToDecimal(item.SpotRate) : 1;

                aPRemarketing.Add(ag);
            }
            return View(aPRemarketing.Distinct());
        }

        public ActionResult ExportApRemarketingReport()
        {
            GridView gv = new GridView();
            var crs = from c in db.Remarketings
                      join f in db.Fleets on c.LogNumber equals f.LogNumber

                      select new
                      {
                          c.FleetNumber,
                          c.UnitNumber,
                          c.LogNumber,
                          c.Penalty,
                          c.GainLoss,
                          c.ProfitShareAmount,
                          c.ComplementaryRent,
                          c.CreditNote,
                          c.Roe,
                          c.SpotRate,
                          c.Status,
                          c.SoldDate,
                          c.BankAccount
                      };
            crs = crs.Where(s => s.Status.ToUpper().Equals("SOLD"));

            List<APRemarketing> aPRemarketing = new List<Models.APRemarketing>();
            foreach (var item in crs)
            {
                APRemarketing ag = new APRemarketing();
                ag.JournalCategory = "?Remarketing";
                ag.JournalSource = "?payables";
                ag.CorpCode = 501;
                ag.CostCentre = "Diana provee info";
                ag.Account = "Diana provee info";
                ag.BankAccount = item.BankAccount;
                ag.PeriodName = "APR-17";
                ag.CRNumber = "Número de transacción";
                ag.DAN = item.LogNumber;
                ag.FleetNumber = item.FleetNumber;
                ag.UnitNumber = item.UnitNumber;
                ag.ChargeCode = "Diana provee info";
                ag.ExternalDocLocator = "Número de factura del activo";
                ag.CorpCodeAP = "N/A";
                ag.ProcessDate = "Fecha de transacción";
                ag.InternalDocumentLocator = "Número de documento contable";
                ag.PartyName = "N/A";
                ag.TransactionDate = Convert.ToDateTime(item.SoldDate);
                ag.EnteredDrSUM = Convert.ToDecimal(item.Penalty) + Convert.ToDecimal(item.ProfitShareAmount);
                ag.EnteredCrSUM = Convert.ToDecimal(item.ComplementaryRent) + Convert.ToDecimal(item.CreditNote);
                ag.NET = ag.EnteredDrSUM - ag.EnteredCrSUM;
                ag.AccountedDrSUM = ag.EnteredDrSUM * item.Roe;
                ag.AccountedCrSUM = ag.EnteredCrSUM * item.Roe;
                ag.CurrencyCode = item.Roe.Equals(0) ? "USD" : "MXN";
                ag.Charge = "N/A";
                ag.TCInvoice = item.Roe.Equals(0) ? item.Roe : 1;
                ag.TCPayment = item.Roe.Equals(0) ? Convert.ToDecimal(item.SpotRate) : 1;

                aPRemarketing.Add(ag);
            }

            gv.DataSource = aPRemarketing.Distinct();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=APRemarketing.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            db.SaveChanges();

            return RedirectToAction("APRemarketing");
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
