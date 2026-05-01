/*
 * OrderFilters.cs
 * FreshHarvestMarket
 *
 * This class defines filter options used by BrowseOrdersViewModel
 * to control which orders are displayed in the orders dashboard.
 *
 * It allows users & administrators to filter orders by status:
 * - Active orders
 * - Past (fulfilled) orders
 * - Cancelled orders
 *
 * Each property determines whether a specific order type is included
 * in the results displayed on the Orders page.
 */

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
