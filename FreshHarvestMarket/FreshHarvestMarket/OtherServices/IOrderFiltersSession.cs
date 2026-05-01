/*
 * IOrderFiltersSession.cs
 * FreshHarvestMarket
 *
 * This interface defines the contract for managing order filter
 * settings within the user session.
 *
 * It provides methods for storing & retrieving filter preferences
 * used on the Browse Orders page, allowing filter selections to persist
 * across requests.
 *
 * Implementations of this interface handle session-based state
 * management for order filtering.
 */

using FreshHarvestMarket.ViewModels;

namespace FreshHarvestMarket.OtherServices
{
    public interface IOrderFiltersSession
    {
        public void SetFilters(BrowseOrdersViewModel viewModel);
        public OrderFilters? GetFilters();
    }
}
