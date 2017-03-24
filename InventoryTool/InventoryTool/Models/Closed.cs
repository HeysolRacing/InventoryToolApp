using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryTool.Models
{
    public class Closed
    {
        public int SupplierCode { get; set; }
        public string Serial { get; set; }
        public string Invoice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public int Currency { get; set; }
        public int ExchangeRate { get; set; }
        public string FleetUnit { get; set; }
        public string Description { get; set; }
        public string CRnumber { get; set; }
        public string Docto { get; set; }
        public string Related { get; set; }
        public string SerialDocRelated { get; set; }
        public string DocRelated { get; set; }
    }
}