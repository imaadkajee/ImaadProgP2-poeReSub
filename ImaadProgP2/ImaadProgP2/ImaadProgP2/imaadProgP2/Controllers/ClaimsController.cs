using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImaadProgP2.Data;
using ImaadProgP2.Models;
using System.Security.Claims;

namespace ImaadProgP2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Programme Coordinator,Academic Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Claims.ToListAsync());
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claims claims, IFormFile? documentFile)
        {
            if (ModelState.IsValid)
            {
                if (documentFile != null && documentFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    var filePath = Path.Combine(uploadsDir, Path.GetFileName(documentFile.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentFile.CopyToAsync(stream);
                    }
                    claims.DocumentPath = filePath;
                }
                else
                {
                    claims.DocumentPath = null;
                }

                _context.Add(claims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(claims);
        }

        [Authorize(Roles = "Lecturer,Programme Coordinator,Academic Manager")]
        public async Task<IActionResult> StatusOverview()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }
    }
}
