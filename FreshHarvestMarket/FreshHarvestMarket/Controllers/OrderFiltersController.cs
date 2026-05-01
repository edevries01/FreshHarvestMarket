/*
 * OrderFiltersController.cs
 * FreshHarvestMarket
 *
 * This controller is responsible for managing filter settings
 * on the Browse Orders screen.
 *
 * It updates filter preferences (such as active, past, & cancelled orders)
 * & stores them in session using the IOrderFiltersSession service.
 *
 * The controller follows the Post-Redirect-Get (PRG) pattern by saving
 * the updated filters & then redirecting back to the main Order view,
 * ensuring a clean URL & preventing duplicate submissions.
 */

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
