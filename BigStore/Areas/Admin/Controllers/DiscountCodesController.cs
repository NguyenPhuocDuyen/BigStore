using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BigStore.BusinessObject;
using BigStore.DataAccess;
using Microsoft.AspNetCore.Authorization;
using BigStore.BusinessObject.OtherModels;

namespace BigStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleContent.Admin)]
    public class DiscountCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscountCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DiscountCodes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DiscountCodes.Include(d => d.DiscountType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/DiscountCodes/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.DiscountCodes == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes
                .Include(d => d.DiscountType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discountCode == null)
            {
                return NotFound();
            }

            return View(discountCode);
        }

        // GET: Admin/DiscountCodes/Create
        public IActionResult Create()
        {
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/DiscountCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DiscountTypeId,Code,Value,MaxValueDiscount,RemainingUsageCount,StartDate,EndDate,CreateAt,UpdateAt,IsDelete")] DiscountCode discountCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "Id", "Name", discountCode.DiscountTypeId);
            return View(discountCode);
        }

        // GET: Admin/DiscountCodes/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.DiscountCodes == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes.FindAsync(id);
            if (discountCode == null)
            {
                return NotFound();
            }
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "Id", "Name", discountCode.DiscountTypeId);
            return View(discountCode);
        }

        // POST: Admin/DiscountCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,DiscountTypeId,Code,Value,MaxValueDiscount,RemainingUsageCount,StartDate,EndDate,CreateAt,UpdateAt,IsDelete")] DiscountCode discountCode)
        {
            if (id != discountCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountCodeExists(discountCode.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountTypeId"] = new SelectList(_context.DiscountTypes, "Id", "Name", discountCode.DiscountTypeId);
            return View(discountCode);
        }

        // GET: Admin/DiscountCodes/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.DiscountCodes == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCodes
                .Include(d => d.DiscountType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discountCode == null)
            {
                return NotFound();
            }

            return View(discountCode);
        }

        // POST: Admin/DiscountCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DiscountCodes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DiscountCodes'  is null.");
            }
            var discountCode = await _context.DiscountCodes.FindAsync(id);
            if (discountCode != null)
            {
                _context.DiscountCodes.Remove(discountCode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountCodeExists(string id)
        {
          return (_context.DiscountCodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
