using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ImaadProgP2.Data;

namespace  ImaadProgP2.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ClaimReport()
        {
            var approvedClaims = await _context.Claims
                .Where(c => c.ApprovalStatus == "Approved")
                .ToListAsync();

            foreach (var claim in approvedClaims)
            {
                claim.TotalAmount = claim.GetTotalAmount();
            }

            return View(approvedClaims);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateInvoice(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);

            if (claim == null)
            {
                return NotFound();
            }

            return View("Invoice", claim);
        }

        [Authorize(Roles = "HR")]
        public IActionResult ManageLecturerData()
        {
            var lecturers = _context.Claims
                .GroupBy(c => new { c.LecturerID, c.FirstName, c.LastName })
                .Select(g => new
                {
                    g.Key.LecturerID,
                    g.Key.FirstName,
                    g.Key.LastName
                })
                .ToList();

            return View(lecturers);
        }

        [Authorize(Roles = "HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLecturerData(string lecturerId, string firstName, string lastName)
        {
            var lecturerClaims = await _context.Claims
                .Where(c => c.LecturerID == lecturerId)
                .ToListAsync();

            if (lecturerClaims.Any())
            {
                foreach (var claim in lecturerClaims)
                {
                    claim.FirstName = firstName;
                    claim.LastName = lastName;
                    _context.Claims.Update(claim);
                }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Lecturer data updated successfully.";
            }
            else
            {
                TempData["Message"] = "Lecturer not found.";
            }

            return RedirectToAction(nameof(ManageLecturerData));
        }
    }
}
