using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Firmeza.web.Data.Entity;
using Firmeza.web.Models; // crearás las vistas-modelo aquí

namespace Firmeza.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                // Verificar contraseña
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,            // ← ESTO hace el cookie persistente
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(model.Email));
                    Console.WriteLine($"[DEBUG] User {model.Email} has roles: {string.Join(", ", roles)}");

                    if (roles.Contains("Cliente"))
                    {
                        await _signInManager.SignOutAsync(); // evitar dejar la sesión iniciada
                        ModelState.AddModelError(string.Empty, "No tiene permisos para acceder al panel administrativo.");
                        return View(model);
                    }

                    if (roles.Contains("Administrador") || roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                return View(model);
            }
            catch (OperationCanceledException)
            {
                ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Por favor, intente nuevamente.");
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Login failed: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error durante el inicio de sesión. Por favor, intente nuevamente más tarde.");
                return View(model);
            }
        }


        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // ✅ Asignar automáticamente el rol "Cliente"
                        await _userManager.AddToRoleAsync(user, "Cliente");

                        // 🚫 No iniciar sesión automáticamente
                        // await _signInManager.SignInAsync(user, isPersistent: false);

                        TempData["Message"] = "Registro exitoso. Por favor, inicie sesión con su cuenta.";
                        return RedirectToAction("Login", "Account");
                    }

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (OperationCanceledException)
                {
                    ModelState.AddModelError(string.Empty, "La solicitud tardó demasiado. Por favor, intente nuevamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Register failed: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Ocurrió un error durante el registro. Por favor, intente nuevamente más tarde.");
                }
            }

            return View(model);
        }



        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
