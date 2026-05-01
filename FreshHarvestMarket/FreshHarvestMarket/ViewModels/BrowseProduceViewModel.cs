/*
 * BrowseProduceViewModel.cs
 * FreshHarvestMarket
 *
 * This ViewModel is used to display & manage the data shown on the
 * produce browsing (marketplace) page.
 *
 * It supports:
 * - Displaying a list of available produce items
 * - Filtering produce by category
 * - Displaying all available categories from the database
 * - Tracking which items are marked as favorites by the user
 *
 * This ViewModel helps power the marketplace UI by combining product
 * data, filtering options, & user-specific favorite selections.
 */


using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    public class BrowseProduceViewModel
    {
        // List of items to show
        public List<Produce> ProduceItems { get; set; } = new();

        // Category the user is currently looking at. Default = All
        public string SelectedCategory { get; set; } = "All";
         
        // List of all categories found in the database
        public List<string> Categories { get; set; } = new();

        public List<int> FavoriteIds { get; set; } = new();
    }
}
