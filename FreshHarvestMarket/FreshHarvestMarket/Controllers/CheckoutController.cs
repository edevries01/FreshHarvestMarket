using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly FreshMarketContext _context;

        public CheckoutController(CartService cartService, FreshMarketContext context)
        {
            _cartService = cartService;
            _context = context;
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

            // CREATE ORDER
            var order = new Order
            {
                OrderDate = DateTime.Now,
                PickupDate = model.PickupDate,
                IsPickedUp = false,
                OrderTotal = cart.Sum(x => x.LineTotal)
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // generates OrderId

            // CREATE ORDER ITEMS
            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProduceId = item.ProduceId,
                    Quantity = item.Quantity
                };

                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            // CLEAR CART
            _cartService.ClearCart();

            // BUILD CONFIRMATION VIEWMODEL
            var confirmation = new ConfirmationViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                PickupDate = model.PickupDate,
                Items = cart
            };

            return View("Confirmation", confirmation);
        }
    }
}