using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryTool.Models
{
    public class Ssupplier
    {
        
        [Key]
        public int SupplierID { get; set; }
        public int OracleID { get; set; }
        public string SID { get; set; }
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }
        [Display(Name = "Legal Name")]
        public string LegalName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country_cd { get; set; }
        [Display(Name = "ZIP Code")]
        public int ZIPCode { get; set; }
        [Display(Name = "Affiliate Store")]
        public string StoreNumber { get; set; }
        [Display(Name = "Account Code")]
        public int NatlAccountCode { get; set; }
        [Display(Name = "Bill Method")]
        public string BillMethod { get; set; }
        public int DiscParts { get; set; }
        public int DiscLabor { get; set; }
        public int PaymentTerms { get; set; }
        public int disc_cap_amt { get; set; }
        public string supplier_typ_cd { get; set; }
        [Display(Name = "Main phone")]
        public string Telephone{ get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Web Link")]
        public string WebLink { get; set; }
        [Display(Name = "E-mail")]
        public string email { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName  { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string TaxID { get; set; }
    }
}