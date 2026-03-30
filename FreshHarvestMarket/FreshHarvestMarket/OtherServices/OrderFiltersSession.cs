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


    }
}
