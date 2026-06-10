using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomerPortal.Models;

namespace CustomerPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Create()
        {
            // Provide defaults so required fields (Country/Role) are populated
            var model = new CustomerPortal.Models.CreateUserViewModel
            {
                Country = "ZAR",
                Role = "User"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerPortal.Models.CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email already in use.");
                return View(model);
            }

            var user = new SystemUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                Country = model.Country,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(model);
            }

            // Ensure role exists and assign
            var role = string.IsNullOrWhiteSpace(model.Role) ? "User" : model.Role;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<CustomerPortal.Models.UserListViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(new CustomerPortal.Models.UserListViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Country = u.Country,
                    Roles = string.Join(", ", roles)
                });
            }

            return View(model);
        }
    }
}
