using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public ViewResult LogIn()
        {
            return View(model: new UserLoginDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            await signInManager.SignOutAsync();

            var user = await userManager.FindByEmailAsync(email: loginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(
                    key: "",
                    errorMessage: "incorrect email or password");
                goto error_exit;
            }

            var loginResult = await signInManager.PasswordSignInAsync(
                user: user,
                password: loginDto.Password,
                isPersistent: false, // todo: to change
                lockoutOnFailure: false);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError(
                    key: "",
                    errorMessage: "incorrect email or password");
                goto error_exit;
            }
            return Redirect(url: "/");

        error_exit:
            return View(model: loginDto);
        }
    }
}
