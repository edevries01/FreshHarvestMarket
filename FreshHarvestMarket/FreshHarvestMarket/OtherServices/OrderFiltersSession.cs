using FreshHarvestMarket.Extensions;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FreshHarvestMarket.OtherServices
{
    public class OrderFiltersSession
    {
        private const string OrderFiltersKey = "OrderFilters";

        private ISession session { get; set; }

        public OrderFiltersSession(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Store filtes from session
        /// We make a new filters object so we don't store the List<order>
        /// </summary>
        /// <param name="filters"></param>
        public void SetFilters(BrowseOrdersViewModel filters)
        {
            session.SetObject(OrderFiltersKey, new
            {
                filters.IncludeActiveOrders,
                filters.IncludeCancelledOrders,
                filters.IncludePastOrders
            });
        }

        /// <summary>
        /// Get the session-stored filter, or a default if none exists
        /// Default is all are true
        /// </summary>
        /// <returns>BrowseOrders filter</returns>
        public BrowseOrdersViewModel GetFilters()
        {
            return session.GetObject<BrowseOrdersViewModel>(OrderFiltersKey) ?? new BrowseOrdersViewModel
            {
                IncludeActiveOrders = true,
                IncludeCancelledOrders = true,
                IncludePastOrders = true
            };
        }
    }
}
