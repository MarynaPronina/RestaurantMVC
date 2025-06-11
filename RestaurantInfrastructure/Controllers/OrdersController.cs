using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestaurantContext _context;
        public OrdersController(RestaurantContext context) => _context = context;

        
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
           
            ViewBag.Dishes = _context.Dishes
                               .Select(d => new { d.Id, d.Name, d.Price })
                               .ToList();
            return View(new Order { DateTime = DateTime.Now });
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Order order,
            int[] DishIds,      
            int[] Quantities)   
        {
            if (!ModelState.IsValid)
            {
                
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
                ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
                ViewBag.Dishes = _context.Dishes.Select(d => new { d.Id, d.Name, d.Price }).ToList();
                return View(order);
            }

            
            order.Id = GenerateNewOrderId();

            
            for (int i = 0; i < DishIds.Length; i++)
            {
                if (Quantities[i] <= 0) continue;
                var dish = await _context.Dishes.FindAsync(DishIds[i]);
                if (dish == null) continue;

                order.DishOrders.Add(new DishOrder
                {
                    DishId = dish.Id,
                    Quantity = Quantities[i],
                    Dish = dish           
                });
            }

           
            order.Sum = order.DishOrders
                .Sum(d => (d.Dish.Price ?? 0) * d.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Table)
                .Include(o => o.DishOrders).ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? NotFound() : View(order);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
            ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
            return View(order);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "LastName", order.ClientId);
                ViewBag.Tables = new SelectList(_context.Tables, "Id", "Number", order.TableId);
                return View(order);
            }

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? NotFound() : View(order);
        }

        [HttpPost, ActionName("DeleteConfirmed"), ValidateAntiForgeryToken]
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

        
        private int GenerateNewOrderId() =>
            _context.Orders.Any() ? _context.Orders.Max(o => o.Id) + 1 : 1;
    }
}
