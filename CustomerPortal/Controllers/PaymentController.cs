using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerPortal.Data;
using CustomerPortal.Models;
using System.Text.RegularExpressions;

namespace CustomerPortal.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class PaymentController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<SystemUser> userManager;
        private readonly ILogger<PaymentController> logger;

        public PaymentController(AppDbContext context, UserManager<SystemUser> userManager, ILogger<PaymentController> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
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
            // Admin sees all payments; regular users see only their own.
            var paymentsQuery = context.Payments
                                       .Include(p => p.User)
                                       .Include(p => p.VerifiedBy)
                                       .OrderByDescending(p => p.CreatedAt)
                                       .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                var userId = userManager.GetUserId(User);
                paymentsQuery = paymentsQuery.Where(p => p.UserId == userId);
            }

            var payments = paymentsQuery.ToList();
            return View(payments);
        }

        // Admin: mark a payment as verified after manual checks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(int id)
        {
            var payment = await context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            if (payment.IsSubmitted)
            {
                TempData["Error"] = "Payment already submitted to SWIFT.";
                return RedirectToAction("MyPayments");
            }

            payment.IsVerified = true;
            payment.VerifiedAt = DateTime.UtcNow;
            payment.VerifiedById = userManager.GetUserId(User);

            await context.SaveChangesAsync();

            TempData["Message"] = $"Payment {payment.Id} verified.";
            return RedirectToAction("MyPayments");
        }

        // Admin: submit selected, verified payments to SWIFT (simulated)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitToSwift(int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Error"] = "No payments selected.";
                return RedirectToAction("MyPayments");
            }

            // Load payments ensuring they belong to selection, are verified and not yet submitted
            var payments = await context.Payments
                .Where(p => selectedIds.Contains(p.Id) && p.IsVerified && !p.IsSubmitted)
                .ToListAsync();

            if (!payments.Any())
            {
                TempData["Error"] = "No verified payments available for submission.";
                return RedirectToAction("MyPayments");
            }

            foreach (var p in payments)
            {
                // Basic SWIFT code sanity check (already enforced by model, extra safety)
                if (!Regex.IsMatch(p.SwiftCode ?? string.Empty, @"^[A-Z0-9]{8,11}$"))
                {
                    logger.LogWarning("Payment {PaymentId} has invalid SWIFT code {Swift}", p.Id, p.SwiftCode);
                    continue;
                }

                // Simulate submission to SWIFT: set status and reference
                p.IsSubmitted = true;
                p.SubmittedAt = DateTime.UtcNow;
                p.Status = "Completed";
                p.SwiftReference = $"SWIFT-{Guid.NewGuid():N}".Substring(0, 20);
            }

            await context.SaveChangesAsync();

            TempData["Message"] = $"Submitted {payments.Count} payment(s) to SWIFT.";
            return RedirectToAction("MyPayments");
        }
    }
}