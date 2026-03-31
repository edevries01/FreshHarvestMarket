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
        /// Whether or not active order should be included in the view
        /// </summary>
        public bool IncludeActiveOrders { get; set; } = true;

        /// <summary>
        /// Whether or not cancelled orders should be included in the view
        /// </summary>
        public bool IncludeCancelledOrders { get; set; } = true;

        /// <summary>
        /// Whether or not to include orders that are past
        /// </summary>
        public bool IncludePastOrders { get; set; } = true;
    }
}
