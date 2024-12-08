using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Fase1PED.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Fase1PED.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreUsuario", login.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Contrasena", login.Contrasena);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        // Validación para evitar DBNull y errores de conversión
                        int userId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0;
                        string nombreUsuario = dr["NombreUsuario"] != DBNull.Value ? dr["NombreUsuario"].ToString() : "UsuarioDesconocido";

                        if (userId > 0) // Confirmamos que el usuario es válido
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, nombreUsuario),
                                new Claim("UserId", userId.ToString())
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                principal,
                                new AuthenticationProperties
                                {
                                    IsPersistent = false, // Cookie no persistente
                                    AllowRefresh = false  // No permite renovaciones de cookies
                                });

                            return RedirectToAction("Index", "Usuario");
                        }
                    }

                    ViewBag.Error = "Nombre de usuario o contraseña incorrectos.";
                    return View(login);
                }
            }

            return View(login);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpia cookies relacionadas con el dominio
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}