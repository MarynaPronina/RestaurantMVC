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

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName");
            ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number");

            var order = new Order
            {
                DateTime = DateTime.Now
            };

            return View(order);
        }

        // POST: Orders/Create
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

            order.Id = GenerateNewOrderId();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
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

        // POST: Orders/Edit/5
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

        // GET: Orders/Details/5
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

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
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

        // ---------- 🆕 Метод генерації нового ID ----------
        private int GenerateNewOrderId()
        {
            if (!_context.Orders.Any())
                return 1;

            return _context.Orders.Max(o => o.Id) + 1;
        }
    }
}
