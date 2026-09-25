using E_Commerce.Entities;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICustomerService _customerService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICustomerService customerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
        }

        public async Task<IdentityResult> RegisterAsync(
            RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(
                user,
                model.Password);

            if (!result.Succeeded)
                return result;

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                "Customer");

            if (!roleResult.Succeeded)
                return roleResult;

            await _customerService.CreateAsync(
                new CustomerViewModel
                {
                    UserId = user.Id
                });

            return IdentityResult.Success;
        }

        public async Task<SignInResult> LoginAsync(
            LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(
                model.Email);

            if (user == null)
                return SignInResult.Failed;

            if (!user.IsActive)
                return SignInResult.NotAllowed;

            return await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Guid> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(
                _signInManager.Context.User);

            if (user == null)
                throw new Exception("User not found.");

            return user.Id;
        }
    }
}
