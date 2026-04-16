// Car Controller.cs

using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cart;

        public CartController(CartService cart)
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            var cart = _cart.GetCart();
            ViewBag.Total = _cart.GetTotal();
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
            _cart.UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }
    }
}