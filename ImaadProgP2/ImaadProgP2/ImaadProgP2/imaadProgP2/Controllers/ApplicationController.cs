using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImaadProgP2.Data;

namespace ImaadProgP2.Controllers;
public class ApprovalController : Controller
{
    private readonly ApplicationDbContext _context;

    public ApprovalController(ApplicationDbContext context)
    {
        _context = context;
    }

   
    public async Task<IActionResult> Review(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null)
        {
            return NotFound();
        }
        return View(claim);
    }

    
    [Authorize(Roles = "Programme Coordinator,Academic Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveConfirmed(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim != null)
        {
            claim.ApprovalStatus = "Approved";
            claim.ApprovedBy = User.Identity.Name;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Claims");
    }

   
    [Authorize(Roles = "Programme Coordinator,Academic Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectConfirmed(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim != null)
        {
            claim.ApprovalStatus = "Rejected";
            claim.ApprovedBy = User.Identity.Name;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Claims");
    }
}
