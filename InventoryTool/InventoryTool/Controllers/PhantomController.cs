using InventoryTool.Models;
using System.IO;
using System.Web.Mvc;
using System.Data;
using System.Linq;
using System;

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
                      select s;
                crs = crs.Where(s => s.Status.Equals("Pending Aproval") || s.Status.Equals("Approved"));

            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    foreach(CR item in crs)
                    {
                        string vin = item.VINnumber;
                        //vin = vin.Substring(6);
                        DateTime date = item.Servicedate;
                        string outputValue =item.Total.ToString("0000000.00");
                        string odometer = item.Odometer.ToString("0000000000");
                        string formatdate = date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
                        string record = item.WAnumber + "|" + vin + "|" + outputValue + "|" + formatdate + "|" + odometer;
                        sw.WriteLine(record);
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
