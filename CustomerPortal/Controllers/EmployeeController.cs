using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerPortal.Data;
using CustomerPortal.Models;

namespace CustomerPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext context;

        public EmployeeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var payments = context.Payments.ToList();
            return View(payments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var payment = await context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            payment.Status = "Approved";
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}