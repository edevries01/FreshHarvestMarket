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

        /// <summary>
        /// OrderController constructor
        /// </summary>
        /// <param name="orderRepo">Service for accessing Order table in database</param>
        public OrderController(IRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        /// <summary>
        /// For now: Returns view with all of the orders. Applies some filters
        /// In Future: After Identity, only return orders for signed-in user
        /// </summary>
        /// <returns>View with display of existing orders</returns>
        public IActionResult Index(BrowseOrdersViewModel model)
        {
            OrderFiltersSession session = new OrderFiltersSession(HttpContext.Session);

            if (session.GetFilters() != null)
            {
                model.Filters = session.GetFilters()!; //Can ignore warning cause we check null right above
            }

            //Grab all the upcoming/pastdue orders
            if (model.Filters.IncludeActiveOrders)
            {
                List<Order> nonPickedUp = _orderRepo.GetAll().Where(o => !o.IsPickedUp).Where(o => !o.Rejected).ToList();
                model.ActiveOrders = nonPickedUp ?? new List<Order>();
            }
            else
            {
                model.ActiveOrders = new List<Order>();
            }

            if (model.Filters.IncludePastOrders)
            {
                //Grab all the fufilled/cancelled orders
                List<Order> pastOrders = _orderRepo.GetAll().Where(o => o.IsPickedUp || o.Rejected).ToList();
                model.PastOrders = pastOrders ?? new List<Order>();
            }
            else 
            {
                model.PastOrders = new List<Order>();
            }

            if (!model.Filters.IncludeCancelledOrders)
            {
                model.PastOrders = model.PastOrders.Where(o => !o.Rejected).ToList();
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
            List<Order> orders = _orderRepo.GetAll().Where(o => !o.IsPickedUp).OrderBy(o => o.PickupDate).ToList();

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
        /// Marks an order as fufilled
        /// </summary>
        /// <param name="orderId">ID of the order fufilled</param>
        /// <returns>Manage Orders view</returns>
        [HttpPost]
        public IActionResult UpdateFufilled(int orderId) 
        {
            Order? order = _orderRepo.GetAll().Where(o => o.OrderId == orderId).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            //Set rejected to true
            order.IsPickedUp = true;

            _orderRepo.Update(order);
            _orderRepo.Save();

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
