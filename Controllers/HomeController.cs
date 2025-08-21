using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Data;
using ProductMgmt.Models;
using System.Threading.Tasks;

namespace ProductMgmt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var totalCategories = await _context.Categories.CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            var recentCategories = await _context.Categories
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToListAsync();

            var recentProducts = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                TotalCategories = totalCategories,
                TotalProducts = totalProducts,
                RecentCategories = recentCategories,
                RecentProducts = recentProducts
            };

            return View(model);
        }
    }

    public class DashboardViewModel
    {
        public int TotalCategories { get; set; }
        public int TotalProducts { get; set; }

        public List<Category> RecentCategories { get; set; } = new();
        public List<Product> RecentProducts { get; set; } = new();
    }
}
