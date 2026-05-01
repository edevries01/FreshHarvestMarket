/*
 * FavoritesViewModel.cs
 * FreshHarvestMarket
 *
 * This ViewModel is used to pass favorite produce data from the controller
 * to the Favorites view.
 *
 * It contains:
 * - A list of favorite Produce items to be displayed
 * - A list of FavoriteIds used to track which items are marked as favorites
 *
 * This allows the view to both display favorite items & determine
 * which items are currently selected as favorites for the logged-in user.
 */

using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    // FavoritesViewModel used to pass collection of favorite produce to view
    public class FavoritesViewModel
    {
        public List<Produce> FavoriteItems { get; set; } = new List<Produce>();

        public List<int> FavoriteIds { get; set; } = new();
    }
}
