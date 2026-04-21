using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.Services
{
    public interface ICartService
    {
        List<CartItem> GetCart();
        void AddItem(CartItem item);
        void RemoveItem(int id);
        void UpdateQuantity(int produceId, int quantity);
        decimal GetTotal();
        void ClearCart();
    }
}
