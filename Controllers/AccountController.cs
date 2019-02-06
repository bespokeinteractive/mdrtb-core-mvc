using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

using EtbSomalia.Models;
using EtbSomalia.Services;
using EtbSomalia.Extensions;
using EtbSomalia.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EtbSomalia.Controllers
{
    public class AccountController : Controller
    {
        [BindProperty]
        public LoginModel Input { get; set; }

        [BindProperty]
        public AccountAddEditUsersViewModel UserEdit { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> Login(LoginModel model, string ReturnUrl = "/") {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            model.ReturnUrl = ReturnUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, UserService Svc, CrytoUtilsExtensions Cryto){
            if (ModelState.IsValid)
            {
                Users user = Svc.GetUser(Input.User.Username); //AuthenticateUser(Input.Email, Input.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    model.Message = "Invalid login attempt.";
                    return View(model);
                }

                if (!user.Enabled){
                    ModelState.AddModelError(string.Empty, "Login account Disabled.");
                    model.Message = "Login account Disabled.";
                    return View(model);
                }

                if (!Cryto.Decrypt(user.Password).Equals(Input.User.Password)){
                    ModelState.AddModelError(string.Empty, "Login Failed. Invalid password.");
                    model.Message = "Login Failed. Invalid password.";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.NewPass)) {
                    if (user.ToChange) { 
                        model.ToChange = 1;
                        return View(model);                    
                    }
                }
                else {
                    user.Password = Cryto.Encrypt(model.NewPass);
                    user.UpdatePassword();
                }

                user.UpdateLastAccess();

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Actor, user.Id.ToString()),
                    new Claim(ClaimTypes.UserData, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.Locality, user.Role.Id.ToString()),
                    new Claim(ClaimTypes.Thumbprint, user.AdminRole.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(Input.ReturnUrl.Trim()))
                    return LocalRedirect(Input.ReturnUrl.Trim());
                return LocalRedirect("/");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied(string ReturnUrl = "") {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [Authorize(Roles = "Administrator, SuperUser")]
        [Route("administrator/users")]
        public IActionResult Users() {
            List<Users> users = new List<Users>(new UserService(HttpContext).GetUsers());
            return View(users);
        }

        [Authorize]
        [Route("administrator/users/add")]
        public IActionResult UsersAdd(AccountAddEditUsersViewModel model) {
            return View(model);
        }

        [Authorize]
        [Route("administrator/users/{idnt}")]
        public IActionResult UsersView(long idnt, UserService service) {
            return View(service.GetUser(idnt));
        }

        [Authorize]
        [Route("administrator/users/edit/{idnt}")]
        public IActionResult UsersEdit(long idnt) {
            return View();
        }

        [AllowAnonymous]
        public int CheckIfUserExists(int usr_idnt, string usr_name) {
            return new UserService().CheckIfUserExists(new Users { Id = usr_idnt, Username = usr_name });
        }

        [HttpPost]
        public IActionResult AddNewUser() {
            Users user = UserEdit.User;

            if (user.Role.Id.Equals(3))
                user.AdminRole = UserEdit.Region;
            else if (user.Role.Id.Equals(4))
                user.AdminRole = UserEdit.Agency;
            else
                user.AdminRole = 0;

            user.Save(HttpContext);

            new UserService(HttpContext).UpdateUsersFacilities(user, UserEdit.Facility);

            return LocalRedirect("/administrator/users/");
        }
    }
}
