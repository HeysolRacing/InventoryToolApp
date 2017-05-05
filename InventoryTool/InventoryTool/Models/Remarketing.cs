using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventoryTool.Models
{
    public class Remarketing
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Fleet Number")]
        public string FleetNumber { get; set; }

        [Display(Name = "Unit Number")]
        public string UnitNumber { get; set; }

        [Display(Name = "Log Number")]
        public int LogNumber { get; set; }

        [Display(Name = "Historical Exchange Rate(ROE)")]
        public decimal Roe { get; set; }

        [Display(Name = "Spot Rate")]
        public decimal? SpotRate { get; set; }

        [Display(Name = "Subcontract Number")]
        public string ScontrNumber { get; set; }

        [Display(Name = "On road Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> OnroadDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> EndDate { get; set; }

        [Display(Name = "Term")]
        public int? Term { get; set; }

        [Display(Name = "Off-road Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> OffroadDate { get; set; }

        [Display(Name = "Current Period")]
        public int? CurrentPeriod { get; set; }

        [Display(Name = "Amortization")]
        public decimal? Amortization { get; set; }

        [Display(Name = "Interest")]
        public decimal? Interest { get; set; }

        [Display(Name = "Rent")]
        public decimal? Rent { get; set; }

        [Display(Name = "Remaining Months")]
        public int? RemainingMonths { get; set; }

        [Display(Name = "Rate")]
        public int? Rate { get; set; }

        [Display(Name = "Penalty")]
        public decimal? Penalty { get; set; }

        [Display(Name = "Sale Value [MXN]")]
        public decimal? SaleValue { get; set; }

        [Display(Name = "Sold Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> SoldDate { get; set; }

        [Display(Name = "Book Value")]
        public decimal? BookValue { get; set; }

        [Display(Name = "Gain/Loss")]
        public decimal? GainLoss { get; set; }

        [Display(Name = "Profit Share amount")]
        public decimal? ProfitShareAmount { get; set; }

        [Display(Name = "Profit Share percentage")]
        public decimal? ProfitSharePercentage { get; set; }

        [Display(Name = "Complementary Rent")]
        public decimal? ComplementaryRent { get; set; }

        [Display(Name = "Credit Note")]
        public decimal? CreditNote { get; set; }

        [Display(Name = "P&L Gain/Loss")]
        public decimal? PLGainLoss { get; set; }

        public string Status { get; set; }

        public string Quote { get; set; }

        [Display(Name = "Outlet Code")]
        public string OutletCode { get; set; }

        [Display(Name = "Outlet Name")]
        public string Outletname { get; set; }

        [Display(Name = "Bank Account")]
        public string BankAccount { get; set; }

        public Nullable<DateTime> Created { get; set; }

        public string CreatedBy { get; set; }
    }
}