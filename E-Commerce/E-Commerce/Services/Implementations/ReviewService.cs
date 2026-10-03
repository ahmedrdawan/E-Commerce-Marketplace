using E_Commerce.Models.Entities;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Review;

namespace E_Commerce.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _uow;

        public ReviewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<ReviewViewModel>> GetForProductAsync(int productId)
        {
            var reviews = await _uow.Reviews.GetByProductAsync(productId);
            return reviews.Select(r => new ReviewViewModel
            {
                CustomerName = r.Customer?.FullName ?? "User",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<(bool Success, string? Error)> AddReviewAsync(string customerId, CreateReviewViewModel viewModel)
        {
            var hasPurchased = await _uow.Reviews.HasPurchasedAsync(customerId, viewModel.ProductId);
            if (!hasPurchased)
                return (false, "You cannot review a product you have not purchased yet (or that has not been delivered yet).");

            var alreadyReviewed = await _uow.Reviews.HasReviewedAsync(customerId, viewModel.ProductId);
            if (alreadyReviewed)
                return (false, "You have already reviewed this product.");

            await _uow.Reviews.AddAsync(new Review
            {
                ProductId = viewModel.ProductId,
                CustomerId = customerId,
                Rating = viewModel.Rating,
                Comment = viewModel.Comment
            });
            await _uow.CompleteAsync();
            return (true, null);
        }
    }
}
