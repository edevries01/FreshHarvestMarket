using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
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
        private FreshMarketContext _context;

        public OrderController(FreshMarketContext ctx)
        {
            _context = ctx;
        }

        /// <summary>
        /// For now: Returns view with all of the orders
        /// In Future: After Identity, only return orders for signed-in user
        /// </summary>
        /// <returns>View with display of existing orders</returns>
        public IActionResult Index()
        {
            List<Order> allOrders = _context.Orders.ToList();

            BrowseOrdersViewModel ordersVM = new BrowseOrdersViewModel();

            //Grab all the upcoming/pastdue orders
            List<Order> nonPickedUp = _context.Orders.Where(o => !o.IsPickedUp).Where(o => !o.Rejected).ToList();
            ordersVM.ActiveOrders = nonPickedUp ?? new List<Order>();

            //Grab all the fufilled/cancelled orders
            List<Order> pastOrders = _context.Orders.Where(o => o.IsPickedUp || o.Rejected).ToList();
            ordersVM.PastOrders = pastOrders ?? new List<Order>();

            return View("BrowseOrders", ordersVM);
        }

        //Add one for admins to view all orders
        /// <summary>
        /// Returns a view for managing orders. Displays all orders, with overdue ones at the top, upcoming ones below.
        /// Has a button for marking an order as fufilled, and another for rejecting an order
        /// </summary>
        /// <returns>View with all unfufilled orders</returns>
        [HttpGet]
        public ViewResult ManageOrders()
        {
            List<Order> orders =  _context.Orders.Where(o => !o.IsPickedUp).OrderBy(o => o.PickupDate).ToList();

            return View(orders);
        }

        /// <summary>
        /// Returns a view with details on a particular order
        /// </summary>
        /// <param name="orderId">The id of the order to view</param>
        /// <returns>A view with details on a particular order</returns>
        [HttpGet]
        public ViewResult ViewOrder(int orderId)
        {
            Order? order = _context.Orders
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
        public ActionResult ConfirmReject(int orderId)
        {
            Order? order = _context.Orders.Find(orderId);

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
        public ActionResult UpdateRejected(int orderId)
        {
            Order? order = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
            
            if (order == null)
            {
                return NotFound();
            }

            //Set rejected to true
            order.Rejected = true;

            _context.Orders.Update(order);
            _context.SaveChanges();
               
            return RedirectToAction("ManageOrders");
        }

        /// <summary>
        /// Marks an order as fufilled
        /// </summary>
        /// <param name="orderId">ID of the order fufilled</param>
        /// <returns>Manage Orders view</returns>
        [HttpPost]
        public ActionResult UpdateFufilled(int orderId) 
        {
            Order? order = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            //Set rejected to true
            order.IsPickedUp = true;

            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("ManageOrders");
        }

        /// <summary>
        /// Returns a view to filter and browse all orders including ones which can no longer be edited
        /// </summary>
        /// <returns></returns>
        public ViewResult BrowseOrders()
        {
            throw new NotImplementedException();
        }
    }
}
