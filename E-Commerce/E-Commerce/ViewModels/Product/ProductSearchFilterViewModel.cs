namespace E_Commerce.ViewModels.Product
{
    public class ProductSearchFilterViewModel
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string? SortByPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
