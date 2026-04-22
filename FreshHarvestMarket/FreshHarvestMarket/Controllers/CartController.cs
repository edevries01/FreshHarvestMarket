// Cart Controller.cs

using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Identity;

namespace FreshHarvestMarket.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        // Note: uncomment when login is applied ---- private readonly UserManager<ApplicationUser> _userManager; // for accessing the user data

        public CartController(ICartService cart)         // Note: Add when login is applied ---- (...., UserManager<ApplicationUser> userManager)
        {
            _cart = cart;
            // Note: uncomment when login is applied ---- _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cart.GetCart();
            var total = _cart.GetTotal();

            /* Uncomment when login is implemented -------
            // If the user is logged in, apply discount
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null) // User is logged in
            {
                total = ApplyDiscount(cart, total); // apply discount logic
            }

            ----------- */
            ViewBag.Total = total; // set the total to the discounted value (if logged in)
            return View(cart);
        }

        // Method to apply discount if user is logged in
        private decimal ApplyDiscount(List<CartItem> cart, decimal total)
        {
            foreach (var item in cart)
            {
                // Assuming a discount model exists for each product, 
                // apply the discount on each item in the cart
                var discount = item.Produce?.Discount;
                if (discount != null)
                {
                    decimal discountAmount = item.Price * discount.DiscountAmount / 100;
                    item.Price -= discountAmount; // apply discount to the item price
                }
            }

            // Recalculate total with discounted prices
            total = cart.Sum(x => x.Price * x.Quantity); // total price with discount
            return total;
        }

        public IActionResult Add(int id, string name, string image, decimal price, int maxQuantity)
        {
            _cart.AddItem(new CartItem
            {
                ProduceId = id,
                ProduceName = name,
                ImageUrl = image,
                Price = price,
                Quantity = 1,
                MaxQuantity = maxQuantity
            });

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            _cart.RemoveItem(id);
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cartItem = _cart.GetCart().FirstOrDefault(x => x.ProduceId == id);

            if (cartItem == null)
                return RedirectToAction("Index");

            if (quantity > cartItem.MaxQuantity)
            {
                TempData["CartError"] = $"Only {cartItem.MaxQuantity} available for {cartItem.ProduceName}.";
                quantity = cartItem.MaxQuantity;
            }

            _cart.UpdateQuantity(id, quantity);

            return RedirectToAction("Index");
        }
    }
}