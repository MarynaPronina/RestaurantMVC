using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestaurantContext _context;

        public OrdersController(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Table)
                .ToListAsync();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName");
            ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,TableId,DateTime")] Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
                ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
                return View(order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
            ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,TableId,DateTime")] Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
                ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
                return View(order);
            }

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Table)
                .Include(o => o.DishOrders).ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
