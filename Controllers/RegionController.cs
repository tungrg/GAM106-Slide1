using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Iana;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    public class RegionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RegionController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var regions = await _context.Regions.ToListAsync();
            return View(regions);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateRegionRequest request)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Regions.AnyAsync(r => r.Name == request.Name))
                {
                    ModelState.AddModelError(string.Empty, "Region with the same name already exists.");
                    return View(request);
                }

                var region = new Models.Region
                {
                    Name = request.Name
                };

                _context.Regions.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [HttpGet("delete")]
        public async Task<IActionResult> Delete()
        {
            var regions = await _context.Regions.ToListAsync();
            return View(regions);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return RedirectToAction("Delete");
        }
    }

}

