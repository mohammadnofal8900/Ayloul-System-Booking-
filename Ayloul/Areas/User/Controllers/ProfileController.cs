using Ayloul.Areas.User.Models;
using Ayloul.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ayloul.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // ==========================================
        // Profile
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // ==========================================
        // Edit Profile - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                Id = user.Id,
                FUllName = user.FUllName,
                PhoneNumber = user.PhoneNumber,
                Note = user.Note,
                Email = user.Email
            };

            return View(model);
        }


        // ==========================================
        // Edit Profile - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(
            EditProfileViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }


            // Get currently logged-in user

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }


            // ==========================================
            // Update Personal Information
            // ==========================================

            user.FUllName = Model.FUllName;
            user.PhoneNumber = Model.PhoneNumber;
            user.Note = Model.Note;


            // ==========================================
            // Change Password
            // ==========================================

            if (!string.IsNullOrWhiteSpace(Model.NewPassword))
            {
                // Current password is required

                if (string.IsNullOrWhiteSpace(Model.CurrentPassword))
                {
                    ModelState.AddModelError(
                        "CurrentPassword",
                        "Please enter your current password."
                    );

                    return View(Model);
                }


                // Change password

                var passwordResult =
                    await _userManager.ChangePasswordAsync(
                        user,
                        Model.CurrentPassword,
                        Model.NewPassword
                    );


                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(
                            "CurrentPassword",
                            error.Description
                        );
                    }

                    return View(Model);
                }
            }


            // ==========================================
            // Update User
            // ==========================================

            var result = await _userManager.UpdateAsync(user);


            if (result.Succeeded)
            {
                TempData["SuccessMessage"] =
                    "Your profile has been updated successfully.";

                return RedirectToAction(nameof(Profile));
            }


            // ==========================================
            // Errors
            // ==========================================

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description
                );
            }


            return View(Model);
        }
    }
}