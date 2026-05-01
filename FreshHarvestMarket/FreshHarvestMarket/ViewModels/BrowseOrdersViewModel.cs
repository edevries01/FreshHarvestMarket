/*
 * BrowseOrdersViewModel.cs
 * FreshHarvestMarket
 *
 * This ViewModel is used to supply data for the Browse Orders page.
 *
 * It organizes orders into three categories:
 * - Active orders (upcoming or overdue orders)
 * - Past orders (completed/fulfilled orders)
 * - Cancelled orders (rejected orders)
 *
 * It also includes filter settings that allow users & administrators
 * to control which order types are displayed on the page.
 */


using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    /// <summary>
    /// Presents data for the BrowseOrders screen and manages the filters 
    /// </summary>
    public class BrowseOrdersViewModel
    {
        /// <summary>
        /// A list of all past/cancelled orders
        /// </summary>
        public List<Order> PastOrders { get; set; } = new List<Order>();

        /// <summary>
        /// A list of all upcoming orders, or past-due orders
        /// </summary>
        public List<Order> ActiveOrders { get; set; } = new List<Order>();

        /// <summary>
        /// A list of all Rejected/Cancelled orders
        /// </summary>
        public List<Order> CancelledOrders { get; set; } = new List<Order>();

        /*
         * Below here, is the stuff for filters
         */

        /// <summary>
        /// The filters for the Browse Orders view
        /// </summary>
        public OrderFilters Filters { get; set; } = new OrderFilters() 
        {
            IncludeActiveOrders = true,
            IncludeCancelledOrders = true,
            IncludePastOrders = true,
        };
    }
}
