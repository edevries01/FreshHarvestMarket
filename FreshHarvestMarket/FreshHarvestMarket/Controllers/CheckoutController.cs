using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Discount> _discountRepo;
        private readonly IRepository<OrderItem> _orderItemRepo;
        private readonly IHttpContextAccessor _contextAccessor;


        public CheckoutController(ICartService cartService, UserManager<User> userManager, IRepository<User> userRepo, 
            IRepository<OrderItem> orderItemRepo, IHttpContextAccessor contextAccessor)
        {
            _cartService = cartService;
            _userManager = userManager;
            _userRepo = userRepo;
            _orderItemRepo = orderItemRepo;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CheckoutViewModel();

            //If they are logged in we can pre-fill some of their information from the User table
            //We expect that the user may choose to change this
            bool loggedIn = _contextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (loggedIn) 
            {
                string userId = _userManager.GetUserId(_contextAccessor.HttpContext.User)!;
                User? authedUser = _userRepo.GetAll().FirstOrDefault(u => u.Id == userId);
    

                if (authedUser != null) 
                {
                    model.Phone = authedUser.PhoneNumber!; //null should not matter
                    model.Email = authedUser.Email!; //null should not matter
                    model.FirstName = authedUser.FirstName!;
                    model.LastName = authedUser.LastName!;
                }
            }
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
                    var discount = _discountRepo.GetAll()
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

            //if signed-in, tie order to user
            if (User.Identity.IsAuthenticated)
            {
                order.UserId = _userManager.GetUserId(User);
            }

            _orderRepo.Insert(order);
            _orderRepo.Save(); // generates OrderId

            // CREATE ORDER ITEMS
            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProduceId = item.ProduceId,
                    Quantity = item.Quantity
                };

                _orderItemRepo.Insert(orderItem);
            }

            _orderItemRepo.Save();

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