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

        public async Task<IActionResult> Index()
        {
            var data = await _context.DishCategories.Include(dc => dc.Dish).Include(dc => dc.Category).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.DishCategories.Include(dc => dc.Dish).Include(dc => dc.Category).FirstOrDefaultAsync(dc => dc.Id == id);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            return View(dishCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory == null) return NotFound();
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            return View(dishCategory);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DishCategory dishCategory)
        {
            if (id != dishCategory.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(dishCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishCategory.DishId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dishCategory.CategoryId);
            return View(dishCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var dishCategory = await _context.DishCategories.Include(dc => dc.Dish).Include(dc => dc.Category).FirstOrDefaultAsync(dc => dc.Id == id);
            return dishCategory == null ? NotFound() : View(dishCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory != null)
            {
                _context.DishCategories.Remove(dishCategory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}