using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SFApp.DTOs;
using SFApp.Services;
using System.Security.Claims;

namespace SFApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsuariosService _usuariosService;

        public AuthController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string codigo, string contrasena, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.Mensaje = "Ingrese código y contraseña.";
                return View();
            }

            var usuario = await _usuariosService.Login(codigo, contrasena);
            if (usuario == null)
            {
                ViewBag.Mensaje = "Código o contraseña incorrectos.";
                return View();
            }

           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("CodigoUsuario", usuario.Codigo) 
            };

            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsPrincipal, 
                new AuthenticationProperties
                {
                    IsPersistent = false
                   
                });

            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View(); 
        }
    }
}
