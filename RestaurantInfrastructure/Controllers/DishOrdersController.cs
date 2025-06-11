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
            var dishOrders = _context.DishOrders
                .Include(d => d.Dish)
                .Include(d => d.Order);
            return View(await dishOrders.ToListAsync());
        }

        // GET: DishOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders
                .Include(d => d.Dish)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dishOrder == null) return NotFound();

            return View(dishOrder);
        }

        // GET: DishOrders/Create
        public IActionResult Create()
        {
            ViewData["Dishes"] = new SelectList(_context.Dishes, "Id", "Name");
            ViewData["Orders"] = new SelectList(_context.Orders, "Id", "Id");
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
                await UpdateOrderSum(dishOrder.OrderId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Dishes"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["Orders"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
            return View(dishOrder);
        }

        // GET: DishOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders.FindAsync(id);
            if (dishOrder == null) return NotFound();

            ViewData["Dishes"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["Orders"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
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
                try
                {
                    _context.Update(dishOrder);
                    await _context.SaveChangesAsync();
                    await UpdateOrderSum(dishOrder.OrderId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishOrderExists(dishOrder.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Dishes"] = new SelectList(_context.Dishes, "Id", "Name", dishOrder.DishId);
            ViewData["Orders"] = new SelectList(_context.Orders, "Id", "Id", dishOrder.OrderId);
            return View(dishOrder);
        }

        // GET: DishOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dishOrder = await _context.DishOrders
                .Include(d => d.Dish)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dishOrder == null) return NotFound();

            return View(dishOrder);
        }

        // POST: DishOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishOrder = await _context.DishOrders.FindAsync(id);
            if (dishOrder != null)
            {
                int orderId = dishOrder.OrderId;
                _context.DishOrders.Remove(dishOrder);
                await _context.SaveChangesAsync();
                await UpdateOrderSum(orderId);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DishOrderExists(int id)
        {
            return _context.DishOrders.Any(e => e.Id == id);
        }

        private async Task UpdateOrderSum(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.DishOrders)
                .ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                order.Sum = order.DishOrders
                    .Sum(d => (d.Dish.Price ?? 0) * d.Quantity);

                await _context.SaveChangesAsync();
            }
        }

    }
}
