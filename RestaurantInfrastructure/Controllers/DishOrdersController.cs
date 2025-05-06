using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class DishOrdersController : Controller
    {
        private readonly RestaurantContext _context;

        public DishOrdersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: DishOrders
        public async Task<IActionResult> Index()
        {
            var dishOrders = await _context.DishOrders
                .Include(d => d.Dish)
                .Include(o => o.Order)
                .ToListAsync();
            return View(dishOrders);
        }

        // GET: DishOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders
                .Include(d => d.Dish)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            return dishOrder == null ? NotFound() : View(dishOrder);
        }

        // GET: DishOrders/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name");
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            return View();
        }

        // POST: DishOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishOrder dishOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
            return View(dishOrder);
        }

        // GET: DishOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders.FindAsync(id);
            if (dishOrder == null) return NotFound();

            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
            return View(dishOrder);
        }

        // POST: DishOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DishOrder dishOrder)
        {
            if (id != dishOrder.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(dishOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
            return View(dishOrder);
        }

        // GET: DishOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders
                .Include(d => d.Dish)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            return dishOrder == null ? NotFound() : View(dishOrder);
        }

        // POST: DishOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishOrder = await _context.DishOrders.FindAsync(id);
            if (dishOrder != null)
            {
                _context.DishOrders.Remove(dishOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
