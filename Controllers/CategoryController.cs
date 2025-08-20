using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Data;
using ProductMgmt.Models;

namespace ProductMgmt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context) => _context = context;

        // GET: /Category
        public IActionResult Index()
        {
            var categories = _context.Categories
                .Include(c => c.AttributeDefinitions) // Include related attributes
                .ToList();
            return View(categories);
        }

        // GET: /Category/Create
        public IActionResult Create() => View(new Category());

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.Slug = category.Name.Trim().ToLower().Replace(" ", "-");
            category.CreatedAt = DateTime.UtcNow;

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            var existing = _context.Categories.Find(category.Id);
            if (existing == null) return NotFound();

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.Slug = category.Name.Trim().ToLower().Replace(" ", "-");
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Details/5
        public IActionResult Details(int id)
        {
            var category = _context.Categories
                .Include(c => c.AttributeDefinitions)
                .FirstOrDefault(c => c.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
