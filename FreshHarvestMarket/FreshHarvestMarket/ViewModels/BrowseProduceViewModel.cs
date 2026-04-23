using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    public class BrowseProduceViewModel
    {
        //List of items to show
        public List<Produce> ProduceItems { get; set; } = new();

        //Category the user is currently looking at. Default = All
        public string SelectedCategory { get; set; } = "All";

        //List of all categories found in the database
        public List<string> Categories { get; set; } = new();

        public List<int> FavoriteIds { get; set; } = new();
    }
}
