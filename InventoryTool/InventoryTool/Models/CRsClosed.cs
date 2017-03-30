using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryTool.Models
{
    public class CRsClosed
    {
        [Display(Name = "Invoice Number")]
        public string Invoicenumber { get; set; }
        public int Supplier { get; set; }
        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Invoicedate { get; set; }
        public int NotAplicable { get; set; }
        public int CurrencyNumber { get; set; }
        public int ExchangeRate { get; set; }
        public string Concept { get; set; }
        public string NotAplicable2 { get; set; }
        public string NotAplicable3 { get; set; }
        public string DeliverDate { get; set; }
        public string ExpirationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,###0.00}")]
        public decimal Subtotal { get; set; }
        public int NotAplicable4 { get; set; }
        public int NotAplicable5 { get; set; }
        public int NotAplicable6 { get; set; }
        public int NotAplicable7 { get; set; }
        public int TaxScheme { get; set; }
        public int ItemCode { get; set; }
        public int NotAplicable8 { get; set; }
        public int IEPS { get; set; }
        public int Tax2 { get; set; }
        public int Tax3 { get; set; }
        public int IVA { get; set; }
        public string CRNumber { get; set; }

    }
}