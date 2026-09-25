namespace E_Commerce.Entities
{
    public class Seller
    {
        public Guid UserId { get; set; }
        public string? StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? StoreLogo { get; set; }
        public string? Address { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
