using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    /// <summary>
    /// Controller concerned with updating the filters for the browse orders screen
    /// </summary>
    public class OrderFiltersController : Controller
    {
        /// <summary>
        /// Updates the filters in session data and then uses PRG to return to browse orders
        /// </summary>
        /// <returns>Redirection to the browse orders view</returns>
        public IActionResult Index(BrowseOrdersViewModel viewModel)
        {
            OrderFiltersSession session = new OrderFiltersSession(HttpContext.Session);

            session.SetFilters(viewModel);

            return RedirectToAction("Index", "Order");
        }
    }
}
