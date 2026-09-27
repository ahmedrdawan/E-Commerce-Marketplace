namespace E_Commerce.ViewModels.Cart
{
    public class ViewCartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.LineTotal);
    }
}
