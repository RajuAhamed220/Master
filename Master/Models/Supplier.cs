using System.ComponentModel;

namespace MasterDetails.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email Address")]
        public string Email { get; set; }
        public IList<Purchase> Purchases { get; set; }


    }
}
