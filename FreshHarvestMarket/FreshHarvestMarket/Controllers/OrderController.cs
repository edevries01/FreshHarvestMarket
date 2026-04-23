using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Controllers
{
    /// <summary>
    /// Controller for Order data entity
    /// </summary>
    public class OrderController : Controller
    {
        private IRepository<Order> _orderRepo;
        private IOrderFiltersSession _orderFiltersSession;
        private IRepository<Produce> _produceRepo;

        /// <summary>
        /// OrderController constructor
        /// </summary>
        /// <param name="orderRepo">Service for accessing Order table in database</param>
        public OrderController(IRepository<Order> orderRepo, IOrderFiltersSession sess, IRepository<Produce> produceRepo)
        {
            _orderRepo = orderRepo;
            _orderFiltersSession = sess;
            _produceRepo = produceRepo;
        }

        /// <summary>
        /// For now: Returns view with all of the orders. Applies some filters
        /// In Future: After Identity, only return orders for signed-in user
        /// </summary>
        /// <returns>View with display of existing orders</returns>
        public IActionResult Index(BrowseOrdersViewModel model)
        {
            if (_orderFiltersSession.GetFilters() != null)
            {
                model.Filters = _orderFiltersSession.GetFilters()!;
            }

            var allOrders = _orderRepo.GetAll().ToList();

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
        public IActionResult ManageOrders()
        {
            List<Order> orders = _orderRepo.GetAll()
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

        /// <summary>
        /// Returns a view to filter and browse all orders including ones which can no longer be edited
        /// </summary>
        /// <returns></returns>
        public IActionResult BrowseOrders()
        {
            throw new NotImplementedException();
        }
    }
}
