﻿using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.UI.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
