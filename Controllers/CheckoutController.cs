using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMgmt.Models;
using ProductMgmt.Data;

namespace ProductMgmt.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart(); 

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart"); 
            }

            ViewBag.Cart = cart;
            return View(new Order()); 
        }        

        [HttpPost]
        public async Task<IActionResult> Index(Order order)
        {
            var cart = GetCart();
            
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            
            if (ModelState.IsValid)
            {
                order.TotalAmount = cart.Items.Sum(i => i.Quantity * i.Product.Price) * 1.1m; 
                
                foreach (var item in cart.Items)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    });
                }
                
                _context.Orders.Add(order);
                
                _context.CartItems.RemoveRange(cart.Items);
                
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Confirmation", new { id = order.Id });
            }
            
            return View(order);
        }

        public IActionResult Confirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);
            
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }

        private Cart? GetCart()
        {
            var userId = HttpContext.Session.GetString("CartUserId");
            
            if (userId == null)
            {
                return null;
            }
            
            return _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);
        }
    }
}