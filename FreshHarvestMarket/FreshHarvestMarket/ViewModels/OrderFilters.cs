namespace FreshHarvestMarket.ViewModels
{
    /// <summary>
    /// Contains the filters of a BrowseOrdersViewModel
    /// </summary>
    public class OrderFilters
    {
        /// <summary>
        /// Whether or not active orders should be displayed
        /// </summary>
        public bool IncludeActiveOrders { get; set; } = true;

        /// <summary>
        /// Whether or not past orders should be displayed
        /// </summary>
        public bool IncludePastOrders { get; set; } = true;

        /// <summary>
        /// Whether or not cancelled orders should be displayed
        /// </summary>
        public bool IncludeCancelledOrders { get; set; } = true;
    }
}
