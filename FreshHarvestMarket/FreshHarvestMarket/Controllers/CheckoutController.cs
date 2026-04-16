using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;

namespace FreshHarvestMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cart;

        public CheckoutController(CartService cart)
        {
            _cart = cart;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CheckoutViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var cart = _cart.GetCart();

            // TODO: save order to DB later
            // (this is where your Order entity gets created)

            _cart.ClearCart(); // optional but recommended

            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}