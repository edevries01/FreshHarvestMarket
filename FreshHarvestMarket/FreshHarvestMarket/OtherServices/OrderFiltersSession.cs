using FreshHarvestMarket.Extensions;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FreshHarvestMarket.OtherServices
{
    /// <summary>
    /// Used to maintain filters on the Browse Orders screen through the session
    /// </summary>
    public class OrderFiltersSession
    {
        private const string OrderFiltersKey = "OrderFilters";

        private ISession session { get; set; }

        public OrderFiltersSession(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Stores the filters in session
        /// </summary>
        /// <param name="viewModel"></param>
        public void SetFilters(BrowseOrdersViewModel viewModel)
        {
            session.SetObject(OrderFiltersKey, viewModel.Filters);
        }

        /// <summary>
        /// Get the session-stored filter, or null if one doesn't exist
        /// </summary>
        /// <returns>BrowseOrders filter</returns>
        public OrderFilters? GetFilters()
        {
            return session.GetObject<OrderFilters>(OrderFiltersKey);
        }
    }
}
