using InventoryTool.Models;
using System.IO;
using System.Web.Mvc;
using System.Data;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace InventoryTool.Controllers
{
    public class PhantomController : Controller
    {
        private InventoryToolContext db = new InventoryToolContext();

        // GET: Phantom
        public ActionResult Index()
        {
            return View();
        }
        // GET: B2B
        public ActionResult B2B()
        {
            return View();
        }
        // GET: Phantom/Search
        public ActionResult Search()
        {
            return RedirectToAction("Phantom", "Fleets");
        }

        // GET: Phantom/List
        public ActionResult List()
        {
            return RedirectToAction("Index", "CRs");
        }

        // GET: Phantom/General
        public ActionResult General()
        {
            return RedirectToAction("General", "CRs");
        }

        // GET: Phantom/Delete/5
        public ActionResult APlist()
        {
            return RedirectToAction("APlist", "CRs");
        }

        public ActionResult txt(string searchString)
        {
            string path = searchString + "B2B.txt";
            //"C:\\estilos\\B2B.txt";

            var crs = from s in db.CRs
                      join f in db.Suppliers on s.Supplier equals f.SupplierID
                      select  new {s.VINnumber, s.WAnumber, s.Servicedate, s.Subtotal, s.Status, f.StoreNumber}; //Cambie Total por SubTotal
                crs = crs.Where(s => s.Status.Equals("Pending Aproval") || s.Status.Equals("Approved")).Distinct();

            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    foreach(var item in crs)
                    {
                        string vin = item.VINnumber;
                        string wa = item.WAnumber.ToUpper();
                        string xwa = "X"+ wa.Substring(2) + "0";
                        string padwa = wa.Insert(0, "".PadLeft(2, ' '));
                        DateTime date = item.Servicedate;
                        string outputValue =item.Subtotal.ToString("0000000.00"); //Cambie Total por SubTotal

                        if (item.StoreNumber != null)
                        {
                            string store = Regex.Match(item.StoreNumber, @"\d+").Value; 
                            uint storenumber = uint.Parse(store);
                            string storenumberfix = storenumber.ToString("0000000000");
                            string formatdate = date.Year.ToString() + date.Month.ToString("00") + date.Day.ToString("00");
                            string record = xwa + "|" + vin + "|" + outputValue + "|" + formatdate + "|" + storenumberfix;
                            sw.WriteLine(record);
                        }
                    }
                   
                    //sw.WriteLine("And");
                    //sw.WriteLine("Welcome");
                    sw.Close();
                }
            }
            return RedirectToAction("B2B");
        }
        
    }
}
