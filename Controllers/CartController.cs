using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Models;
using ProductMgmt.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ProductMgmt.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetOrCreateCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int quantity = 1)
        {
            var cart = GetOrCreateCart();
            var product = await _context.Products.FindAsync(productId);
            
            if (product == null)
            {
                return NotFound();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                });
            }
            
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return Json(new { success = true, itemCount = cart.Items.Sum(i => i.Quantity) });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(int itemId, int quantity)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            
            if (item == null)
            {
                return NotFound();
            }
            
            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            var cart = GetOrCreateCart();
            var count = cart.Items.Sum(i => i.Quantity);
            return Json(count);
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cart = GetOrCreateCart();
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private Cart GetOrCreateCart()
        {
            var userId = GetUserId();
            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefault(c => c.UserId == userId);
            
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            
            return cart;
        }

        private string GetUserId()
        {
            var userId = HttpContext.Session.GetString("CartUserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartUserId", userId);
            }
            
            return userId;
        }
    }
}