using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    //FavoritesViewModel used to pass collection of favorite produce to view
    public class FavoritesViewModel
    {
        public List<Produce> FavoriteItems { get; set; } = new List<Produce>();
    }
}
