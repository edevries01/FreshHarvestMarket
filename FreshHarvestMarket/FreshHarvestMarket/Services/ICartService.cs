/*
 * ICartService.cs
 * FreshHarvestMarket
 *
 * This interface defines the contract for managing shopping cart
 * functionality within the application.
 *
 * It provides methods for:
 * - Retrieving the current cart
 * - Adding & removing items
 * - Updating item quantities
 * - Calculating the total cost
 * - Clearing the cart
 * - Persisting cart data
 *
 * Implementations of this interface handle the business logic for
 * cart operations & allow for flexibility in how cart data is stored.
 */

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
        public void SaveCart(List<CartItem> cart);
    }
}
