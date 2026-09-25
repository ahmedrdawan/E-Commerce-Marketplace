using E_Commerce.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.SeedData
{
    public static class SeedData
    {
        public static async Task Initialize(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                await SeedRoles(roleManager);
                await SeedUsers(userManager);
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {

            var admin = await userManager.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    FullName = "System Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (!result.Succeeded)
                {
                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(
                                ", ",
                                result.Errors.Select(e => e.Description)
                            )
                        );
                    }
                }
                var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description)
                        )
                    );
                }
            }
        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roles =
            {
                    "Admin",
                    "Customer",
                    "Seller"
                };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new Role
                    {
                        Name = role
                    });

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(
                                ", ",
                                result.Errors.Select(e => e.Description)
                            )
                        );
                    }
                }
            }
        }
    }
}
