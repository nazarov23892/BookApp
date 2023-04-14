using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;
using Domain;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return Redirect(url: "/");
        }

        [HttpGet]
        public ViewResult Register(string returnUrl = null)
        {
            return View(model: new UserRegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            if (!String.Equals(
                a: userRegisterModel.Password,
                b: userRegisterModel.ConfirmPassword))
            {
                ModelState.AddModelError(
                    key: "",
                    errorMessage: "passwords are not equals");
                goto error_exit;
            }
            AppUser newUser = new AppUser
            {
                Email = userRegisterModel.Email,
                UserName = userRegisterModel.Email
            };
            var userResult = await userManager.CreateAsync(
                user: newUser,
                password: userRegisterModel.Password);
            if (!userResult.Succeeded)
            {
                AddModelErrors(errors: userResult.Errors);
                goto error_exit;
            }
            var roleResult = await userManager.AddToRoleAsync(
                user: newUser,
                role: DomainConstants.UsersRoleName);
            if (!roleResult.Succeeded)
            {
                AddModelErrors(errors: roleResult.Errors);
                goto error_exit;
            }
            return Redirect(url: "/");

        error_exit:
            return View(model: userRegisterModel);
        }

        private void AddModelErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(
                    key: "",
                    errorMessage: error.Description);
            }
        }
    }
}
