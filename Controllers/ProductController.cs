using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Data;
using ProductMgmt.Models;
using ProductMgmt.Services;

namespace ProductMgmt.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductValidationService _validationService;

        public ProductController(ApplicationDbContext context, ProductValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: List
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.CategoryAttributeDefinition)
                .ToList();
            return View(products);
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Include(c => c.AttributeDefinitions).ToList();
            return View(new Product());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, [FromForm] Dictionary<int, string> attributeValues, IFormFile? ImageFile)
        {
            var (isValid, errors) = _validationService.ValidateProduct(product, attributeValues ?? new Dictionary<int, string>());
            foreach (var err in errors) ModelState.AddModelError(err.Key, err.Value);

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.Include(c => c.AttributeDefinitions).ToList();
                return View(product);
            }

            // Save image if uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }

            product.Slug = product.Name.Trim().ToLower().Replace(" ", "-");
            product.CreatedAt = DateTime.UtcNow;

            _context.Products.Add(product);
            _context.SaveChanges();

            // Save dynamic attributes
            if (attributeValues != null)
            {
                foreach (var kvp in attributeValues)
                {
                    _context.ProductAttributeValues.Add(new ProductAttributeValue
                    {
                        ProductId = product.Id,
                        CategoryAttributeDefinitionId = kvp.Key,
                        Value = kvp.Value ?? string.Empty
                    });
                }
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products
                .Include(p => p.AttributeValues)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.Include(c => c.AttributeDefinitions).ToList();
            ViewBag.AttributeValues = product.AttributeValues
                .ToDictionary(av => av.CategoryAttributeDefinitionId, av => av.Value);

            return View(product);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, [FromForm] Dictionary<int, string> attributeValues, IFormFile? ImageFile)
        {
            var (isValid, errors) = _validationService.ValidateProduct(product, attributeValues ?? new Dictionary<int, string>());
            foreach (var err in errors) ModelState.AddModelError(err.Key, err.Value);

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.Include(c => c.AttributeDefinitions).ToList();
                return View(product);
            }

            var existing = _context.Products.Include(p => p.AttributeValues).FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.SKU = product.SKU;
                existing.Price = product.Price;
                existing.CategoryId = product.CategoryId;
                existing.UpdatedAt = DateTime.UtcNow;

                // Update image if uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    existing.ImageUrl = "/images/products/" + fileName;
                }

                // Update dynamic attributes
                _context.ProductAttributeValues.RemoveRange(existing.AttributeValues);

                if (attributeValues != null)
                {
                    foreach (var kvp in attributeValues)
                    {
                        _context.ProductAttributeValues.Add(new ProductAttributeValue
                        {
                            ProductId = existing.Id,
                            CategoryAttributeDefinitionId = kvp.Key,
                            Value = kvp.Value ?? string.Empty
                        });
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.CategoryAttributeDefinition)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Include(p => p.AttributeValues).FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.ProductAttributeValues.RemoveRange(product.AttributeValues);
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Dynamic attributes for category
        public IActionResult GetAttributesForCategory(int categoryId)
        {
            var attributes = _context.CategoryAttributeDefinitions
                .Where(a => a.CategoryId == categoryId)
                .Select(a => new {
                    a.Id,
                    a.Name,
                    a.DataType,
                    a.IsRequired
                })
                .ToList();

            return Json(attributes);
        }
    }
}
