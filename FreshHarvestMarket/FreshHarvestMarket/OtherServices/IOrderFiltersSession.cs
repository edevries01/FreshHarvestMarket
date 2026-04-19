using FreshHarvestMarket.ViewModels;

namespace FreshHarvestMarket.OtherServices
{
    public interface IOrderFiltersSession
    {
        public void SetFilters(BrowseOrdersViewModel viewModel);
        public OrderFilters? GetFilters();
    }
}
