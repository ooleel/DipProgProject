using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SeniorLearnWebApp.Models;
using SeniorLearnWebApp.Data;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SeniorLearnWebApp.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form)
        {
            var firstName = form["FirstName"];
            var lastName = form["LastName"];
            var email = form["Email"];
            var phone = form["Phone"];
            var dobParsed = DateTime.TryParse(form["DateOfBirth"], out var dob);
            var password = form["Password"];
            var confirmPassword = form["ConfirmPassword"];

            if (!dobParsed || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || password != confirmPassword)
            {
                ModelState.AddModelError("", "Please complete all required fields correctly.");
                return View();
            }

            // Check if a member with this email already exists
            var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.Email == email.ToString());
            if (existingMember != null)
            {
                ModelState.AddModelError("", "A member with this email already exists.");
                return View();
            }

            var tempUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            // Validate password BEFORE saving Member
            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, tempUser, password);
                if (!validationResult.Succeeded)
                {
                    foreach (var error in validationResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            // Now it's safe to save the Member
            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                DateOfBirth = dob
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            // Add MemberRole (Standard)
            var role = new MemberRole
            {
                MemberId = member.MemberId,
                Role = MemberRole.MemberRoleType.Standard,
                StartDate = DateTime.UtcNow
            };

            _context.MemberRoles.Add(role);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                MemberId = member.MemberId
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Welcome");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            [FromForm] string email,
            [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                ModelState.AddModelError(nameof(email), "Email is required");
            else if (!new EmailAddressAttribute().IsValid(email))
                ModelState.AddModelError(nameof(email), "Invalid email format");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError(nameof(password), "Password is required");

            if (!ModelState.IsValid)
            {
                ViewData["Email"] = email;
                return View();
            }

            var result =
                await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Welcome", "Account", new { area = "Identity" });
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewData["Email"] = email;
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            var user = await _userManager.Users
                .Include(u => u.Member)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user?.Member != null)
            {
                ViewData["FirstName"] = user.Member.FirstName;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}