using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Data;
using ProductMgmt.Models;

namespace ProductMgmt.Controllers
{
    public class AttributeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AttributeController(ApplicationDbContext context) => _context = context;

        // List all attributes
        public IActionResult Index()
        {
            var attrs = _context.CategoryAttributeDefinitions.Include(a => a.Category).ToList();
            return View(attrs);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryAttributeDefinition attr)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", attr.CategoryId);
                return View(attr);
            }

            attr.Slug = attr.Name.Trim().ToLower().Replace(" ", "-");
            attr.CreatedAt = DateTime.UtcNow;

            _context.CategoryAttributeDefinitions.Add(attr);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var attr = _context.CategoryAttributeDefinitions.Find(id);
            if (attr == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", attr.CategoryId);
            return View(attr);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryAttributeDefinition attr)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", attr.CategoryId);
                return View(attr);
            }

            var existing = _context.CategoryAttributeDefinitions.Find(attr.Id);
            if (existing == null) return NotFound();

            existing.Name = attr.Name;
            existing.Slug = attr.Name.Trim().ToLower().Replace(" ", "-");
            existing.IsRequired = attr.IsRequired;
            existing.DataType = attr.DataType;
            existing.DisplayOrder = attr.DisplayOrder;
            existing.CategoryId = attr.CategoryId;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public IActionResult Details(int id)
        {
            var attr = _context.CategoryAttributeDefinitions
                .Include(a => a.Category)
                .FirstOrDefault(a => a.Id == id);

            if (attr == null) return NotFound();
            return View(attr);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var attr = _context.CategoryAttributeDefinitions.Find(id);
            if (attr != null)
            {
                _context.CategoryAttributeDefinitions.Remove(attr);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
