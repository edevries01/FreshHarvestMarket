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
        private IOrderFiltersSession _session;

        public OrderFiltersController(IOrderFiltersSession orderFiltersSession) 
        {
            _session = orderFiltersSession;
        }

        /// <summary>
        /// Updates the filters in session data and then uses PRG to return to browse orders
        /// </summary>
        /// <returns>Redirection to the browse orders view</returns>
        public IActionResult Index(BrowseOrdersViewModel viewModel)
        {
            _session.SetFilters(viewModel);

            return RedirectToAction("Index", "Order");
        }
    }
}
