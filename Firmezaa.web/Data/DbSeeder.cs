using Firmeza.web.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.web.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Administrador", "Cliente" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Crear usuario administrador
        string adminEmail = "admin@firmeza.com";
        string adminPassword = "Admin123*";

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
}