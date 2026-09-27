using E_Commerce.ViewModels.Review;

namespace E_Commerce.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IReadOnlyList<ReviewViewModel>> GetForProductAsync(int productId);
        Task<(bool Success, string? Error)> AddReviewAsync(string customerId, CreateReviewViewModel viewModel);
    }
}
