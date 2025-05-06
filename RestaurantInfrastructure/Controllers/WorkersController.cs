// WorkersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;

namespace RestaurantInfrastructure.Controllers
{
    public class WorkersController : Controller
    {
        private readonly RestaurantContext _context;
        public WorkersController(RestaurantContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var workers = await _context.Workers.Include(w => w.Client).ToListAsync();
            return View(workers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _context.Workers.Include(w => w.Client).FirstOrDefaultAsync(w => w.Id == id);
            return worker == null ? NotFound() : View(worker);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Worker worker)
        {
            if (!ModelState.IsValid) return View(worker);
            _context.Add(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _context.Workers.FindAsync(id);
            return worker == null ? NotFound() : View(worker);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Worker worker)
        {
            if (id != worker.Id) return NotFound();
            if (!ModelState.IsValid) return View(worker);
            _context.Update(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _context.Workers.FindAsync(id);
            return worker == null ? NotFound() : View(worker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null) _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
