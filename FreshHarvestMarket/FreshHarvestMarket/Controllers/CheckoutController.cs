using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;

namespace FreshHarvestMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;

        public CheckoutController(CartService cartService)
        {
            _cartService = cartService;
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

            var cart = _cartService.GetCart();

            var confirmation = new ConfirmationViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                PickupDate = model.PickupDate,
                Items = cart
            };

            _cartService.ClearCart();

            return View("Confirmation", confirmation);
        }
        // TODO: save order to DB later
        // (this is where Order entity gets created)

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}