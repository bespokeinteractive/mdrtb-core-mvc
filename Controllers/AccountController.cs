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
using System.Net.Mail;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EtbSomalia.Controllers
{
    public class AccountController : Controller
    {
        [BindProperty]
        public LoginModel Input { get; set; }

        [BindProperty]
        public AccountUsersAddEditViewModel UserEdit { get; set; }

        [BindProperty]
        public AccountFacilitiesViewModel FacilityEdit { get; set; }

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

        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator")]
        public IActionResult Admin() {
            return View();
        }

        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator/users")]
        public IActionResult Users() {
            List<Users> users = new List<Users>(new UserService(HttpContext).GetUsers());
            return View(users);
        }

        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator/users/{idnt}")]
        public IActionResult UsersView(long idnt, AccountUsersViewModel model, UserService service) {
            model.User = service.GetUser(idnt);
            
            return View(model);
        }

        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator/users/add")]
        public IActionResult UsersAdd(AccountUsersAddEditViewModel model) {
            model.Facilities = new UserService().GetUsersFacilitiesAll(new Users());
            return View(model);
        }


        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator/users/edit/{idnt}")]
        public IActionResult UsersEdit(long idnt, AccountUsersAddEditViewModel model, UserService service) {
            model.User = service.GetUser(idnt);
            model.Facilities = service.GetUsersFacilitiesAll(model.User);

            return View(model);
        }

        [Authorize(Roles = "Administrator, Super User")]
        [Route("administrator/facilities")]
        public IActionResult Facilities(AccountFacilitiesViewModel model, CoreService service) {
            model.Facilities = service.GetFacilities();
            model.Agencies = service.GetAgenciesIEnumerable();
            model.Regions = service.GetRegionsIEnumerable();

            return View(model);
        }

        /* Data Readers */
        [AllowAnonymous]
        public int CheckIfUserExists(int usr_idnt, string usr_name) {
            return new UserService().CheckIfUserExists(new Users { Id = usr_idnt, Username = usr_name });
        }

        [AllowAnonymous]
        public int CheckIfFacilityExists(int fac_idnt, string fac_prefix, string fac_name) {
            return new CoreService().CheckIfFacilityExists(new Facility { Id = fac_idnt, Name = fac_name, Prefix = fac_prefix });
        }

        [AllowAnonymous]
        public string ResetPassword(long usr_idnt) {
            new UserService().GetUser(usr_idnt).ResetPassword();
            return "success";
        }

        [AllowAnonymous]
        public string EnableAccount(long usr_idnt, int usr_opts) {
            bool opts = usr_opts != 0;
            new Users(usr_idnt).EnableAccount(opts);
            return "success";
        }

        [AllowAnonymous]
        public JsonResult GetFacility(long idnt) {
            return Json(new CoreService().GetFacility(idnt));
        }

        [AllowAnonymous]
        public string DeleteFacility(long idnt) {
            new Facility(idnt).Delete();
            return "success";
        }


        /* HttpPost */
        [HttpPost]
        public IActionResult AddEditUser() {
            Users user = UserEdit.User;
            Boolean isNew = false || user.Id.Equals(0);

            if (user.Role.Id.Equals(3))
                user.AdminRole = UserEdit.Region;
            else if (user.Role.Id.Equals(4))
                user.AdminRole = UserEdit.Agency;
            else
                user.AdminRole = 0;

            user.Save(HttpContext);

            new UserService(HttpContext).UpdateUsersFacilities(user, UserEdit.Facility);

            if (isNew) {
                MailSendExtensions mail = new MailSendExtensions();
                mail.SendTo.Add(new MailAddress(user.Email, user.Name));
                mail.Subject = "Account created on EtbSomalia";

                string message = "Dear " + user.Name + System.Environment.NewLine + System.Environment.NewLine;
                message += "A new Account has been created for you on EtbSomalia System. Your login credentials are as below" + System.Environment.NewLine;
                message += "URL: http://etbsomalia.worldvision.or.ke" + System.Environment.NewLine;
                message += "Username: " + user.Username + System.Environment.NewLine;
                message += "Password: pass" + System.Environment.NewLine + System.Environment.NewLine;
                message += "You will be prompted to change the password after the first login. Provide a password of your liking." + System.Environment.NewLine + System.Environment.NewLine;
                message += "Regards," + System.Environment.NewLine;
                message += "System Admin" + System.Environment.NewLine + System.Environment.NewLine;
                message += "P.S. This is a system generated Email. Do not respond to it.";

                mail.Message = message;
                mail.Send();
            }

            return LocalRedirect("/administrator/users/");
        }

        [HttpPost]
        public IActionResult AddEditFacility() {
            Facility facility = FacilityEdit.Facility;
            if (string.IsNullOrWhiteSpace(facility.Description))
                facility.Description = "N/A";
            facility.Save(HttpContext);

            return LocalRedirect("/administrator/facilities");
        }
    }
}
