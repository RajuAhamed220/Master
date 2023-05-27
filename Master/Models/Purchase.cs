using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MasterDetails.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }
        [DisplayName("Purchase Code")]
        public string PurchaseCode { get; set; }
        [DisplayName("Purchase Type")]
        public string PurchaseType { get; set; }

        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [DisplayName(" Total AMount")]
        public double? TotalAmount { get; set; }
        [DisplayName("Discount Percentage")]
        public double? DiscountPercentage{get;set; }
        [DisplayName("Discount Amount")]
        public double? DiscountAmount { get; set; }
        [DisplayName("Vat Percentage")]
        public double? VatPercentage { get; set; }
        [DisplayName("Vat Amount")]
        public double? VatAmount { get; set; }
        [DisplayName("Payment Amount")]
        public double? PaymentAmount { get; set; }
        public virtual List<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();

    }
}
