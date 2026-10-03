namespace E_Commerce.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string?> SaveProductImageAsync(IFormFile? file);
        void DeleteProductImage(string? relativeUrl);
    }
}
