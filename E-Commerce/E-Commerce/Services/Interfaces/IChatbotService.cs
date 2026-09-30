namespace E_Commerce.Services.Interfaces
{
    public interface IChatbotService
    {
        Task<string> GetReplyAsync(string message, string? userId);
    }
}