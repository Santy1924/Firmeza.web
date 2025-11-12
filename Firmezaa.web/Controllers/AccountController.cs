using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Firmeza.web.Models; // crearás las vistas-modelo aquí

namespace Firmeza.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
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

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(model);
            }

            // Verificar contraseña
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Obtener roles del usuario
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Cliente"))
                {
                    // Bloquear acceso al panel Razor
                    ModelState.AddModelError(string.Empty, "No tiene permisos para acceder al panel administrativo.");
                    return View(model);
                }

                // Iniciar sesión normalmente si es Administrador
                await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            return View(model);
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
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
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
