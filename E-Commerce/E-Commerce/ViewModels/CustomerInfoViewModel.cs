namespace E_Commerce.ViewModels
{
    public class CustomerInfoViewModel
    {
        public Guid Id { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
