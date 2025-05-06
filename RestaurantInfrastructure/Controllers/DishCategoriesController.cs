using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class DishCategoriesController : Controller
    {
        private readonly RestaurantContext _context;

        public DishCategoriesController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: DishCategories
        public async Task<IActionResult> Index()
        {
            var restaurantContext = _context.DishCategories.Include(d => d.Category).Include(d => d.Dish);
            return View(await restaurantContext.ToListAsync());
        }

        // GET: DishCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategories
                .Include(d => d.Category)
                .Include(d => d.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            return View(dishCategory);
        }

        // GET: DishCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            return View();
        }

        // POST: DishCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,DishId")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            return View(dishCategory);
        }

        // GET: DishCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            return View(dishCategory);
        }

        // POST: DishCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,DishId")] DishCategory dishCategory)
        {
            if (id != dishCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishCategoryExists(dishCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            return View(dishCategory);
        }

        // GET: DishCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategories
                .Include(d => d.Category)
                .Include(d => d.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            return View(dishCategory);
        }

        // POST: DishCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory != null)
            {
                _context.DishCategories.Remove(dishCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishCategoryExists(int id)
        {
            return _context.DishCategories.Any(e => e.Id == id);
        }
    }
}
