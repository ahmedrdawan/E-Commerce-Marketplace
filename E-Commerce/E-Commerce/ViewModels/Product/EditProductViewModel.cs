namespace E_Commerce.ViewModels.Product
{
    public class EditProductViewModel : CreateProductViewModel
    {
        public int Id { get; set; }
        public string? ExistingImageUrl { get; set; }
    }
}
