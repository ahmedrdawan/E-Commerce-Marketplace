using E_Commerce.Services.Interfaces;

namespace E_Commerce.Services.Implementations
{
    // Saves uploaded product images under wwwroot/uploads/products with a safe,
    // random file name
    public class FileStorageService : IFileStorageService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxSizeBytes = 5 * 1024 * 1024; // 5 MB

        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> SaveProductImageAsync(IFormFile? file)
        {
            if (file is null || file.Length == 0)
                return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("Image type not allowed. Allowed types: jpg, jpeg, png, webp");

            if (file.Length > MaxSizeBytes)
                throw new InvalidOperationException("Image size exceeds 5 MB");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/products/{fileName}";
        }

        public void DeleteProductImage(string? relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
