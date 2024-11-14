using System.ComponentModel.DataAnnotations;

namespace server.Application.Models
{
    public class AppliedVouchers
    {
        [Key]
        public int ID{ get; set; }
        public CartProducts? CartProduct { get; set; }
        public Voucher? Voucher { get; set; }
        public AppliedVouchers() { }
    }
}
