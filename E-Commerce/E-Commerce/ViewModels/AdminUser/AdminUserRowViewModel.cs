using E_Commerce.Enums;

namespace E_Commerce.ViewModels.AdminUser
{
    public class AdminUserRowViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public SellerRequestStatus SellerStatus { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
