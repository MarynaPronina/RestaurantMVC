// TablesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class TablesController : Controller
    {
        private readonly RestaurantContext _context;
        public TablesController(RestaurantContext context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.Tables.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
            return table == null ? NotFound() : View(table);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Table table)
        {
            if (!ModelState.IsValid) return View(table);
            _context.Add(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var table = await _context.Tables.FindAsync(id);
            return table == null ? NotFound() : View(table);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Table table)
        {
            if (id != table.Id) return NotFound();
            if (!ModelState.IsValid) return View(table);
            _context.Update(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var table = await _context.Tables.FindAsync(id);
            return table == null ? NotFound() : View(table);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null) _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
