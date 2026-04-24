using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly FreshHarvestContext _context;

        public CheckoutController(ICartService cartService, FreshHarvestContext context)
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

            bool isLoggedIn = User.Identity!.IsAuthenticated;

            if (isLoggedIn)
            {
                foreach (var item in cart)
                {
                    var discount = _context.Discounts
                        .FirstOrDefault(d => d.ProduceId == item.ProduceId);

                    item.DiscountAmount = discount?.DiscountAmount ?? 0;
                }
            }
            else
            {
                foreach (var item in cart)
                {
                    item.DiscountAmount = 0;
                }
            }

            // CREATE ORDER
            var order = new Order
            {
                OrderDate = DateTime.Now,
                PickupDate = model.PickupDate,
                IsPickedUp = false,
                // Apply discount if user is logged in
                OrderTotal = User.Identity!.IsAuthenticated
                    ? cart.Sum(x => x.DiscountedPrice * x.Quantity)  // Discounted price if logged in
                    : cart.Sum(x => x.LineTotal)  // Regular price if not logged in
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

            var finalCart = _cartService.GetCart();

            // CLEAR CART
            _cartService.ClearCart();

            // BUILD CONFIRMATION VIEWMODEL
            var confirmation = new ConfirmationViewModel
            {
                OrderId = order.OrderId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                PickupDate = model.PickupDate,
                Items = finalCart
            };

            return View("Confirmation", confirmation);
        }
    }
}