using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryTool.Models
{
    public class APMaintenance
    {
       public string JournalCategory { get; set; }
       public string JournalSource { get; set; }
       public int CorpCode { get; set; }
       public string CostCentre { get; set; }
       public string Account { get; set; }
       public string Bankaccount { get; set; }
       public string PeriodName { get; set; }
       public string CRNumber { get; set; }
       public int DAN { get; set; }
       public string FleetNumber { get; set; }
       public string UnitNumber { get; set; }
       public string ChargeCode { get; set; }
       public string ExternalDocLocator { get; set; }
       public string CorpCodeAP { get; set; }
       public string ProcessDate { get; set; }
       public string InternalDocumentLocator { get; set; }
       public string PartyName { get; set; }
       public DateTime TransactionDate { get; set; }
       public decimal EnteredDrSUM { get; set; }
       public decimal EnteredCrSUM { get; set; }
       public decimal NET { get; set; }
       public decimal AccountedDrSUM { get; set; }
       public decimal AccountedCrSUM { get; set; }
       public string CurrencyCode { get; set; }
       public bool charge { get; set; }
       public int TCInvoice { get; set; }
       public int TCPayment { get; set; }
    }
}