using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Data;
using ProductMgmt.Models;

namespace ProductMgmt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Category
        public IActionResult Index()
        {
            var categories = _context.Categories.Include(c => c.AttributeDefinitions).ToList();
            return View(categories);
        }

        // GET: /Category/Create
        public IActionResult Create() => View(new Category());

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(category);

            category.Slug = category.Name.Trim().ToLower().Replace(" ", "-");
            category.CreatedAt = DateTime.UtcNow;

            // Save uploaded image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images/categories");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                ImageFile.CopyTo(stream);

                category.ImageUrl = "/images/categories/" + fileName;
            }

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
        public IActionResult Edit(Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(category);

            var existing = _context.Categories.Find(category.Id);
            if (existing == null) return NotFound();

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.Slug = category.Name.Trim().ToLower().Replace(" ", "-");
            existing.UpdatedAt = DateTime.UtcNow;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images/categories");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                ImageFile.CopyTo(stream);

                existing.ImageUrl = "/images/categories/" + fileName;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Details/5
        public IActionResult Details(int id)
        {
            var category = _context.Categories.Include(c => c.AttributeDefinitions)
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
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var filePath = Path.Combine(_env.WebRootPath, category.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
