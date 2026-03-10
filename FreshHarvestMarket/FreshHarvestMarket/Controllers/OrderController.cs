using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
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
            throw new NotImplementedException();
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
    }
}
