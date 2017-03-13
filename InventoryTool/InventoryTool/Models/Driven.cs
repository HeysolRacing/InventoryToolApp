using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryTool.Models
{
    public class Driven
    {  
        [Key]
        public int ID { get; set; }
        public string CostumerReference { get; set; }
        public string ClientUnit { get; set; }
        public int VRN { get; set; }
        public string DriverName { get; set; }
        public string DriverLastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZIP { get; set; }
    }
}