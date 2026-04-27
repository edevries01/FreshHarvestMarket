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

        public CartController(ICartService cart)        
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            var cart = _cart.GetCart();
            var total = _cart.GetTotal();

            ViewBag.Total = total; 
            return View(cart);
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