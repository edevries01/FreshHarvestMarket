/*
 * OrderFiltersSession.cs
 * FreshHarvestMarket
 *
 * This service manages the persistence of order filter settings
 * for the Browse Orders page using session storage.
 *
 * It allows filter selections (active, past, cancelled) to be saved
 * & restored across requests, providing a consistent user experience.
 *
 * The service uses session extension methods to serialize &
 * deserialize the OrderFilters object.
 */


using FreshHarvestMarket.Extensions;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FreshHarvestMarket.OtherServices
{
    /// <summary>
    /// Used to maintain filters on the Browse Orders screen through the session
    /// </summary>
    public class OrderFiltersSession : IOrderFiltersSession
    {
        private const string OrderFiltersKey = "OrderFilters";

        private ISession session { get; set; }

        public OrderFiltersSession(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
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
