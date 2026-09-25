using E_Commerce.Entities;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<Guid> GetCurrentUser();
    }
}
