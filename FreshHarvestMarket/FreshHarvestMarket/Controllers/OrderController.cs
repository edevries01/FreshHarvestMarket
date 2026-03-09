using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    /// <summary>
    /// Controller for Order data entity
    /// </summary>
    public class OrderController : Controller
    {
        /// <summary>
        /// For now: Returns view with all of the orders
        /// In Future: After Identity, only return orders for signed-in user
        /// </summary>
        /// <returns>View with display of existing orders</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
