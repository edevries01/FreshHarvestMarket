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
