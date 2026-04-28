using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Controllers
{
    /// <summary>
    /// Controller for Order data entity
    /// </summary>
    [Authorize]
    public class OrderController : Controller
    {
        private IRepository<Order> _orderRepo;
        private IOrderFiltersSession _orderFiltersSession;
        private IRepository<Produce> _produceRepo;
        private UserManager<User> _userManager;

        /// <summary>
        /// OrderController constructor
        /// </summary>
        /// <param name="orderRepo">Service for accessing Order table in database</param>
        public OrderController(IRepository<Order> orderRepo, IOrderFiltersSession sess, IRepository<Produce> produceRepo, UserManager<User> userManager)
        {
            _orderRepo = orderRepo;
            _orderFiltersSession = sess;
            _produceRepo = produceRepo;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays view with all orders for admins or just the user's orders for regular users
        /// </summary>
        /// <returns>View with display of existing orders</returns>
        public IActionResult Index(BrowseOrdersViewModel model)
        {
            if (_orderFiltersSession.GetFilters() != null)
            {
                model.Filters = _orderFiltersSession.GetFilters()!;
            }

            //Cannot be null, due to [Authorize] requiring auth on this controller
            string userId = _userManager.GetUserId(User)!;

            //Get all orders if admin, user's orders if regular user
            List<Order> allOrders;
            if (User.IsInRole("Admin"))
            {
                allOrders = _orderRepo.GetAll().ToList();
            }
            else
            {
                allOrders = _orderRepo.GetAll().Where(o => o.UserId == userId).ToList();
            }
            

            // -------------------------
            // ACTIVE (only not finished)
            // -------------------------
            if (model.Filters.IncludeActiveOrders)
            {
                model.ActiveOrders = allOrders
                    .Where(o => !o.IsPickedUp && !o.Rejected)
                    .ToList();
            }
            else
            {
                model.ActiveOrders = new List<Order>();
            }

            // -------------------------
            // PAST (FULFILLED ONLY)
            // -------------------------
            if (model.Filters.IncludePastOrders)
            {
                model.PastOrders = allOrders
                    .Where(o => o.IsPickedUp && !o.Rejected)
                    .ToList();
            }
            else
            {
                model.PastOrders = new List<Order>();
            }

            // -------------------------
            // CANCELLED (REJECTED ONLY)
            // -------------------------
            if (model.Filters.IncludeCancelledOrders)
            {
                model.CancelledOrders = allOrders
                    .Where(o => o.Rejected)
                    .ToList();
            }
            else
            {
                model.CancelledOrders = new List<Order>();
            }

            return View("BrowseOrders", model);
        }

        //Add one for admins to view all orders
        /// <summary>
        /// Returns a view for managing orders. Displays all orders, with overdue ones at the top, upcoming ones below.
        /// Has a button for marking an order as fufilled, and another for rejecting an order
        /// </summary>
        /// <returns>View with all unfufilled orders</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageOrders()
        {
            List<Order> orders = _orderRepo.GetAll()
                .Include(o => o.Items)
                .ThenInclude(i => i.Produce)
                .Where(o => !o.IsPickedUp && !o.Rejected)
                .OrderBy(o => o.PickupDate)
                .ToList();

            return View(orders);
        }

        /// <summary>
        /// Returns a view with details on a particular order
        /// </summary>
        /// <param name="orderId">The id of the order to view</param>
        /// <returns>A view with details on a particular order</returns>
        [HttpGet]
        public IActionResult ViewOrder(int orderId)
        {
            Order? order = _orderRepo.GetAll()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Produce)
                .Where(o => o.OrderId == orderId)
                .FirstOrDefault();

            if (order == null)
            {
                RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        /// <summary>
        /// Returns a view where the operator chooses they want to reject an order
        /// </summary>
        /// <param name="orderId"></param>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ConfirmReject(int orderId)
        {
            Order? order = _orderRepo.GetAll().FirstOrDefault(o => o.OrderId == orderId);

            if (order == null) 
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// Updates an order's rejection status
        /// Redirects the operator back to the order management page
        /// </summary>
        /// <returns>Manage Orders view</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRejected(int orderId)
        {
            Order? order = _orderRepo.GetAll().Where(o => o.OrderId == orderId).FirstOrDefault();
            
            if (order == null)
            {
                return NotFound();
            }

            //Set rejected to true
            order.Rejected = true;

            _orderRepo.Update(order);
            _orderRepo.Save();
               
            return RedirectToAction("ManageOrders");
        }

        /// <summary>
        /// Marks an order as fulfilled
        /// </summary>
        /// <param name="orderId">ID of the order fulfilled</param>
        /// <returns>Manage Orders view</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateFulfilled(int orderId)
        {
            Order? order = _orderRepo.GetAll()
                .Include(o => o.Items)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            if (order.IsPickedUp)
                return RedirectToAction("ManageOrders");

            // hard stop validation when fulfilling
            foreach (var item in order.Items)
            {
                var produce = _produceRepo.GetAll()
                    .FirstOrDefault(p => p.ProduceId == item.ProduceId);

                if (produce == null)
                    continue;

                if (produce.InventoryTotal < item.Quantity)
                {
                    TempData["Error"] =
                        $"Cannot fulfill order. Not enough inventory for {produce.ProduceName}.";
                    return RedirectToAction("ManageOrders");
                }
            }

            // Loop through order items & reduce inventory
            foreach (var item in order.Items)
            {
                var produce = _produceRepo.GetAll()
                    .FirstOrDefault(p => p.ProduceId == item.ProduceId);

                if (produce != null)
                {
                    produce.InventoryTotal -= item.Quantity;

                    // prevents negative inventory
                    if (produce.InventoryTotal < 0)
                    {
                        produce.InventoryTotal = 0;
                    }
                }
            }

            // Mark order as fulfilled
            order.IsPickedUp = true;

            _orderRepo.Update(order);
            _orderRepo.Save();

            // Save inventory changes
            _produceRepo.Save();

            return RedirectToAction("ManageOrders");
        }
    }
}
