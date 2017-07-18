﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PayFor.Models;
using PayFor.ViewModels;

namespace PayFor.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<User> _signInManager;

        public AuthController(SignInManager<User> signInManager )
        {
            _signInManager = signInManager;
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);
                if (signInResult.Succeeded)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("","Incorrect UserName or Password!");
            }
            return View();
        } 
        public async  Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
               await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
