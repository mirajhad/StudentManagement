﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.Services;
using StudentManagement.Models;
using System.Security.Claims;
using System.Text.Json;

namespace StudentManagement.UI.Controllers
{
    public class AccountsController : Controller
    {
        private IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            LoginViewModel vm = _accountService.Login(model);
            if(vm != null)
            {
                string sessionObj = JsonSerializer.Serialize(vm);
                HttpContext.Session.SetString("loginDetails", sessionObj);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, model.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return RedirectToUser(vm);
            }
            return View(model);
        }

        private IActionResult RedirectToUser(LoginViewModel vm)
        {
            if(vm.Role == (int)EnumRoles.Admin)
            {
                return RedirectToAction("Index", "Users");
            }else if(vm.Role == (int)EnumRoles.Teacher)
            {
                return RedirectToAction("Index", "Exams");
            }
            else
            {
                return RedirectToAction("Index", "Students");
            }
        }
    }
}
