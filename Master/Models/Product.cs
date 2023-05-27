using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterDetails.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }

        public string Description { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseItems { get; set; }


    }
}
