using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryTool.Models
{
    public class ExchangeRate
    {
        [Key]
        public int ExchangeRateID { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 99.99)]
        public decimal Exchange { get; set; }

        [Display(Name = "Exchange Date")]
        public DateTime Exchangedate { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

    }
}