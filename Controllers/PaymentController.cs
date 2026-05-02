using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerPortal.Data;
using CustomerPortal.Models;

namespace CustomerPortal.Controllers
{
    [Authorize(Roles = "User")]
    public class PaymentController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<SystemUser> userManager;

        public PaymentController(AppDbContext context, UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,Currency,AccountNumber,SwiftCode")] Payment model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            model.UserId = user.Id;
            model.Status = "Pending";
            model.CreatedAt = DateTime.UtcNow;

            context.Payments.Add(model);
            await context.SaveChangesAsync();

            return RedirectToAction("MyPayments");
        }

        [HttpGet]
        public IActionResult MyPayments()
        {
            var userId = userManager.GetUserId(User);

            var payments = context.Payments
                .Where(p => p.UserId == userId)
                .ToList();

            return View(payments);
        }
    }
}