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
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace InventoryTool.Controllers
{
    public class FeeCodesController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();
        public string trans, trans2;

        // GET: FeeCodes
        [Authorize(Roles = "FeeCodesView")]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, 
                                              string currentUnit, string searchUnit, string currentLlogNo, string searchLogNo,
                                              string currentFee, string searchFee,
                                              string InitialDate, string InitialFilter, string FinalDate, string FinalFilter)
        {
            try
            {
                int inicio = 0, final = 0, pos1 = 0, pos2 = 0;
                bool band = true;

                if ((!String.IsNullOrEmpty(InitialDate)) || (!String.IsNullOrEmpty(FinalDate)))
                {
                    if (String.IsNullOrEmpty(InitialDate))
                    {
                        this.HttpContext.Session["Display1"] = "The Initial Date cannot be empty, please set a correct date.";
                        band = false;
                    }
                  

                    if (String.IsNullOrEmpty(FinalDate))
                    { 
                        this.HttpContext.Session["Display1"] = "The Final Date cannot be empty, please set a correct date.";
                        band = false;
                    }

                    if (band)
                    {
                        if (Convert.ToDateTime(FinalDate) < Convert.ToDateTime(InitialDate))
                        {
                            this.HttpContext.Session["Display1"] = "The Initial Date cannot be major than Final Date, please set a correct date.";
                            band = false;
                        }
                        else if (Convert.ToDateTime(FinalDate) > Convert.ToDateTime(InitialDate).AddDays(7))
                        {
                            this.HttpContext.Session["Display1"] = "The Final Date cannot be more than 7 days major than Initial Date, please set a correct date.)";
                            band = false;
                        }
                    }
                }

                if (((!String.IsNullOrEmpty(searchString)) || (!String.IsNullOrEmpty(searchUnit)) || (!String.IsNullOrEmpty(searchLogNo)) || (!String.IsNullOrEmpty(searchFee)) ||
                    (!String.IsNullOrEmpty(InitialDate)) || (!String.IsNullOrEmpty(FinalDate) || (!String.IsNullOrEmpty(currentFilter)) || (!String.IsNullOrEmpty(currentUnit)) || 
                    (!String.IsNullOrEmpty(currentLlogNo)) || (!String.IsNullOrEmpty(currentFee)) || (!String.IsNullOrEmpty(InitialFilter)) || (!String.IsNullOrEmpty(FinalFilter)))) && band)
                {
                    ViewBag.CurrentSort = sortOrder;

                    ViewBag.UnitSortParm = String.IsNullOrEmpty(sortOrder) ? "Unit" : "";
                    ViewBag.FeeSortParm = String.IsNullOrEmpty(sortOrder) ? "Fee" : "";
                    ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "MMYY" : "";
                    ViewBag.LogNoSortParm = String.IsNullOrEmpty(sortOrder) ? "LogNo" : "";


                    if ((searchString != null) || (searchUnit != null) || (searchLogNo != null) || (searchFee != null))
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                        searchUnit = currentUnit;
                        searchLogNo = currentLlogNo;
                        searchFee = currentFee;
                        InitialDate = InitialFilter;
                        FinalDate = FinalFilter;
                    }

                    ViewBag.currentFilter = searchString;
                    ViewBag.currentUnit = searchUnit;
                    ViewBag.currentLlogNo = searchLogNo;
                    ViewBag.currentFee = searchFee;
                    ViewBag.InitialFilter = InitialDate;
                    ViewBag.FinalFilter = FinalDate;

                    var fleets = from s in db.FeeCodes
                                 select s;

                    //Busqueda por fechas
                    if ((!String.IsNullOrEmpty(InitialDate)) && (!String.IsNullOrEmpty(FinalDate)))
                    {
                        //fecha inicial
                        trans = InitialDate.Replace("-", "");
                        inicio = Convert.ToInt32(trans);

                        //Fecha final
                        trans = FinalDate.Replace("-", "");
                        final = Convert.ToInt32(trans);

                        fleets = fleets.Where(s => (s.MMYY >= inicio) && (s.MMYY <= final));
                    }

                    if (!String.IsNullOrEmpty(searchString))
                        fleets = fleets.Where(s => s.Fleet.ToString().Contains(searchString));

                    if (!String.IsNullOrEmpty(searchUnit))
                        fleets = fleets.Where(s => s.Unit.ToString().Contains(searchUnit));

                    if (!String.IsNullOrEmpty(searchLogNo))
                        fleets = fleets.Where(s => s.LogNo.ToString().Contains(searchLogNo));

                    if (!String.IsNullOrEmpty(searchFee))
                        fleets = fleets.Where(s => s.Fee.ToString().Contains(searchFee));

                    switch (sortOrder)
                    {
                        case "Fee":
                            fleets = fleets.OrderByDescending(s => s.Fee);
                            break;
                        case "Unit":
                            fleets = fleets.OrderBy(s => s.Unit);
                            break;
                        case "MMYY":
                            fleets = fleets.OrderByDescending(s => s.MMYY);
                            break;
                        case "LogNo":
                            fleets = fleets.OrderByDescending(s => s.LogNo);
                            break;
                        default:
                            fleets = fleets.OrderBy(s => s.Fleet);
                            break;
                    }
                    this.HttpContext.Session["Display1"] = this.HttpContext.Session["Display2"] = "";
                    int pageSize = 100;
                    int pageNumber = (page ?? 1);
                    return View(fleets.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    if (band)
                    {
                        this.HttpContext.Session["Display1"] = "You must set filters";
                    }
                    else
                    {
                        this.HttpContext.Session["Display1"] =  this.HttpContext.Session["Display2"] = "";
                    }
                    var fleets = from s in db.FeeCodes
                                 where s.LogNo.Equals(0)
                                 select s;
                    int pageSize = 100;
                    int pageNumber = (page ?? 1);
                    return View(fleets.ToPagedList(pageNumber, pageSize));
                }
            }
            catch (Exception ex)
            {
                this.HttpContext.Session["Display1"] = "Error: " + ex.Message + " Try again with more filters"; 
                return RedirectToAction("Index");
            }
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

        [HttpPost]
        [Authorize(Roles = "InventoryUpload")]
        public ActionResult Importexcel()
        {
            string cadenaconexionSQL, line, strsql;
            cadenaconexionSQL = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryToolContext"].ConnectionString;
            SqlConnection conn = new SqlConnection(cadenaconexionSQL);

            bool band = false;
            string Resultado = string.Empty;

            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);

                    if (extension.ToUpper().Trim().Equals(".CSV"))
                    {
                        //string path1 = string.Format("{0}/{1}", Server.MapPath("~/UploadedFiles"), Request.Files["FileUpload1"].FileName);
                        string path1 = @"\SSIS\FeeCodes\" + (Request.Files["FileUpload1"].FileName);
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        Request.Files["FileUpload1"].SaveAs(path1);

                        // Read the file, correct and fill table Cappings
                        strsql = "EXEC [dbo].[sp_CargaCappings] '" + path1 + "'";
                        conn.Open();
                        SqlCommand com = new SqlCommand();
                        com.CommandText = strsql;
                        com.CommandTimeout = 0;
                        com.CommandType = CommandType.Text;
                        com.Connection = conn;
                        SqlDataReader reader = com.ExecuteReader();
                        int i = reader.FieldCount;
                        if (i > 0)
                        {
                            while (reader.Read())
                            {
                                Resultado = reader[0].ToString();
                            }
                        }
                        else
                            Resultado = "There were problems with the charge of FeeCodes, please call IT Area";
                        conn.Close();
                        conn.Dispose();
                    }
                    else
                        Resultado = "The file selected is not in the right format (.CSV)";
                }
                else
                    Resultado = "The file selected is not in the right format (.CSV)";

                this.HttpContext.Session["Display1"] = "";
                this.HttpContext.Session["Display2"] = Resultado;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(@"C:\SSIS\FeeCodes\Error.txt"))
                    System.IO.File.Delete(@"C:\SSIS\FeeCodes\Error.txt");
                StreamWriter sw = new StreamWriter(@"C:\SSIS\FeeCodes\Error.txt");
                String contenido = "Error: " + ex.Message;
                sw.WriteLine(contenido.ToString());
                sw.Close();
                sw.Dispose();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "InventoryExport")]
        public ActionResult ExportData(string searchString, string searchUnit, string searchLogNo, string searchFee, string InitialDate, string FinalDate)
        {
            try
            {
                string sqltxt = string.Empty, trans = string.Empty;
                int inicio = 0, final = 0, pos1 = 0, pos2 = 0;

                if ((!String.IsNullOrEmpty(searchString)) || (!String.IsNullOrEmpty(searchUnit)) || (!String.IsNullOrEmpty(searchLogNo)) || (!String.IsNullOrEmpty(searchFee)) ||
                    (!String.IsNullOrEmpty(InitialDate)) || (!!String.IsNullOrEmpty(FinalDate)))
                {
                    GridView gv = new GridView();

                    var fleets = from s in db.FeeCodes
                                 select s;

                    if ((!String.IsNullOrEmpty(InitialDate)) && (!String.IsNullOrEmpty(FinalDate)))
                    {
                        //fecha inicial
                        trans = InitialDate.Replace("-", "");
                        inicio = Convert.ToInt32(trans);

                        //Fecha final
                        trans = FinalDate.Replace("-", "");
                        final = Convert.ToInt32(trans);

                        fleets = fleets.Where(s => (s.MMYY >= inicio) && (s.MMYY <= final));
                    }

                    if (!String.IsNullOrEmpty(searchString))
                        fleets = fleets.Where(s => s.Fleet.ToString().Equals(searchString));

                    if (!String.IsNullOrEmpty(searchUnit))
                        fleets = fleets.Where(s => s.Unit.ToString().Equals(searchUnit));

                    if (!String.IsNullOrEmpty(searchLogNo))
                        fleets = fleets.Where(s => s.LogNo.ToString().Equals(searchLogNo));

                    if (!String.IsNullOrEmpty(searchFee))
                        fleets = fleets.Where(s => s.Fee.ToString().Equals(searchFee));

                    gv.DataSource = fleets.ToList();
                    gv.DataBind();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=FeeCodesAll.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                    this.HttpContext.Session["Display1"] = "You must set filters";
            }
            catch (Exception ex)
            {
                this.HttpContext.Session["Display1"] = "Error: " + ex.Message + " Try again with more filters";
            }
            return RedirectToAction("Index");
        }
    }
}
