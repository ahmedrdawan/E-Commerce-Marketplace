using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels.Order
{
    public class CheckoutViewModel
    {
        [Required, MaxLength(300)]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
