﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using AutoMapper;
using PayFor.Models;
using PayFor.ViewModels;

namespace PayFor.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IConfiguration _config;

        public AuthController(SignInManager<User> signInManager,
                                UserManager<User> userManager,
                                IConfiguration config )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (signInResult.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("","Incorrect UserName or Password!");
            }
            return View();
        } 
    
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        var claims = await GetValidClaims(user);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                            _config["Tokens:Issuer"],
                            claims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) , name = user.UserName});
                    }
                }
            }
            return BadRequest("Could not create token");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(model.Email) != null) return BadRequest("This email already used!");
                
                var user = new User() { 
                    UserName = model.FirstName, 
                    Email = model.Email,
                    FirstName =  model.FirstName ,
                    LastName =  model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded){
                    return RedirectToAction("GenerateToken",new LoginViewModel{Email = model.Email, Password=model.Password});
                }
                return BadRequest("Something went wrong!");
            }
            return BadRequest("Fill all required fields!");
        }

        //not used
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        //not used
        public async  Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
               await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<List<Claim>> GetValidClaims(User user)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                    new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName)
                };
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            // foreach (var userRole in userRoles)
            // {
            //     claims.Add(new Claim(ClaimTypes.Role, userRole));
            //     var role = await _roleManager.FindByNameAsync(userRole);
            //     if(role != null)
            //     {
            //         var roleClaims = await _roleManager.GetClaimsAsync(role);
            //         foreach(Claim roleClaim in roleClaims)
            //         {
            //             claims.Add(roleClaim);
            //         }
            //     }
            // }
            return claims;
        }
    }
}
