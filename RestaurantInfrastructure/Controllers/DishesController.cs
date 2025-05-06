using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class DishesController : Controller
    {
        private readonly RestaurantContext _context;

        public DishesController(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.Include(d => d.DishCategories).ThenInclude(dc => dc.Category).ToListAsync();
            return View(dishes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.Include(d => d.DishCategories).ThenInclude(dc => dc.Category).FirstOrDefaultAsync(d => d.Id == id);
            return dish == null ? NotFound() : View(dish);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.FindAsync(id);
            return dish == null ? NotFound() : View(dish);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish dish)
        {
            if (id != dish.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            return dish == null ? NotFound() : View(dish);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
