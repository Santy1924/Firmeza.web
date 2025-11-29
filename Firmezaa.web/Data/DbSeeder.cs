using Firmeza.web.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.web.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Administrador", "Cliente" };

            foreach (var role in roles)
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Timeout creating role '{role}'. Database may be unavailable.");
                    return; // Exit early on timeout
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating role '{role}': {ex.Message}");
                }
            }

            // Crear usuario administrador
            string adminEmail = "admin@firmeza.com";
            string adminPassword = "Admin123*";

            try
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Timeout creating admin user. Database may be unavailable.");
                return; // Exit early on timeout
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating admin user: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database seeding failed: {ex.Message}");
            Console.WriteLine("The application will continue running. Database seed may not have completed.");
        }
    }
}