﻿using System;
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
using System.Data.SqlClient;

namespace InventoryTool.Controllers
{
    public class FeeCodesController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: FeeCodes
        [Authorize(Roles = "FeeCodesView")]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, string InitialDate, string InitialFilter,
                                                string FinalDate, string FinalFilter)
        {
            int inicio = 0, final = 0, pos1 = 0, pos2 = 0;
            string trans;

            ViewBag.CurrentSort = sortOrder;
            

            ViewBag.UnitSortParm = String.IsNullOrEmpty(sortOrder) ? "UNIT" : "";
            ViewBag.FeeSortParm = String.IsNullOrEmpty(sortOrder) ? "FEE" : "";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "MMYY" : "";


            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            if (InitialDate != null)
                page = 1;
            else
                InitialDate = InitialFilter;

            if (FinalDate != null)
                page = 1;
            else
                FinalDate = FinalFilter;

            ViewBag.CurrentFilter = searchString;
            ViewBag.InitialFilter = InitialDate;
            ViewBag.FinalFilter = FinalDate;

            //Busqueda por fechas


            if (String.IsNullOrEmpty(InitialDate))
                inicio = 19000101;
            else              // 3/6/2017 
            {
                pos1 = InitialDate.IndexOf('/');
                pos2 = InitialDate.LastIndexOf('/');
                trans = InitialDate.Substring(pos2 + 1);
                if (pos1 == 1)
                    trans += "0" + InitialDate.Substring(0, 1);
                else
                    trans += InitialDate.Substring(0, 2);

                if (pos2 - pos1 <= 2)
                    trans += "0" + InitialDate.Substring(pos1 + 1, 1);
                else
                    trans += InitialDate.Substring(pos1 + 1, 2);
                inicio = Convert.ToInt32(trans);

            }

            if (String.IsNullOrEmpty(FinalDate))
                final = Convert.ToInt32(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00"));
            else
            {
                pos1 = FinalDate.IndexOf('/');
                pos2 = FinalDate.LastIndexOf('/');
                trans = FinalDate.Substring(pos2 + 1);
                if (pos1 == 1)
                    trans += "0" + FinalDate.Substring(0, 1);
                else
                    trans += FinalDate.Substring(0, 2);

                if (pos2 - pos1 <= 2)
                    trans += "0" + FinalDate.Substring(pos1 + 1, 1);
                else
                    trans += FinalDate.Substring(pos1 + 1, 2);
                final = Convert.ToInt32(trans);
            }
                

            var fleets = from s in db.FeeCodes
                     where s.MMYY >= inicio && s.MMYY <= final
                     select s;

            if (!String.IsNullOrEmpty(searchString))
                fleets = fleets.Where(s => s.Fee.ToString().Equals(searchString) || s.Fleet.ToString().Equals(searchString) || s.Unit.ToString().Equals(searchString) || s.LogNo.ToString().Equals(searchString));
            else
                fleets = fleets.Take(100000000);

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

            int pageSize = 20;
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

        public ActionResult Import()
        {
            return View();
        }


        public ActionResult Importexcel()
        {
            string cadenaconexionSQL, line, strsql;
            cadenaconexionSQL = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryToolContext"].ConnectionString;
            SqlConnection conn = new SqlConnection(cadenaconexionSQL);
            SqlCommand com = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int counter = 1;
            bool band = false;
            string[] datos;

            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                //C:\Proyectos\GitHub\FeeCodes\InventoryToolApp\InventoryTool\InventoryTool\UploadedFiles
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/UploadedFiles"), Request.Files["FileUpload1"].FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);

                Request.Files["FileUpload1"].SaveAs(path1);

                conn.Open();
                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(path1);
                string dato = file.ReadLine();
                while (((line = file.ReadLine()) != null) && !band)
                {
                    datos = line.Split(',');
                    //Primero verifico si ya existe el registro
                    strsql = "Select Fleet From FeeCodes Where Fleet = " + Convert.ToInt32(datos[0]) + " And Lpis = " + Convert.ToInt32(datos[7]);
                    strsql += " And Fee = " + Convert.ToInt32(datos[10]);
                    da = new SqlDataAdapter(strsql, conn);
                    ds = new DataSet();
                    da.Fill(ds, "Feecodes");
                    if (ds.Tables[0].Rows.Count <= 0)    //no existe
                    {
                        //Grabo
                        strsql = "Insert into FeeCodes (Fleet, Unit, LogNo, CapCost, BookValue, Rental, Term, Lpis, Scontr, InsPremium, Fee, Descr, MMYY, ";
                        strsql += "Star, Sto, Amt, Method, Rate, BL, AC, Createdby, Created) values(" + Convert.ToInt32(datos[0]) + "," + Convert.ToInt32(datos[1]) + ",";
                        strsql += Convert.ToInt32(datos[2]) + "," + Convert.ToDecimal(datos[3]) + "," + Convert.ToDecimal(datos[4]) + ",";
                        strsql += Convert.ToDecimal(datos[5]) + "," + Convert.ToInt32(datos[6]) + "," + Convert.ToInt32(datos[7]) + ",'" + datos[8] + "',";
                        strsql += Convert.ToDecimal(datos[9]) + "," + Convert.ToInt32(datos[10]) + ",'" + datos[11] + "',";
                        strsql += Convert.ToInt32(datos[12]) + "," + Convert.ToInt32(datos[13]) + "," + Convert.ToInt32(datos[14]) + ",";
                        strsql += Convert.ToDecimal(datos[15]) + ",'" + datos[16] + "','" + datos[17] + "','";
                        strsql += datos[18] + "','" + datos[19] + "','MACRO PROCESS','" + DateTime.Now.ToString() + "')";

                        com = new SqlCommand();
                        com.CommandText = strsql;
                        com.CommandTimeout = 0;
                        com.CommandType = CommandType.Text;
                        com.Connection = conn;
                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        this.HttpContext.Session["Display"] = "There were FeeCodes duplicated, please review";
                        band = true;
                    }
                    counter++;
                }

                file.Close();
                file.Dispose();
                conn.Close();
                conn.Dispose();

                if (!band)
                    this.HttpContext.Session["Display"] = "FeeCodes imported succesfullly";  
            }
            
            return RedirectToAction("Index");
        }

    }
}
