using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using FreshHarvestMarket.Data;
using FreshHarvestMarket.Repositories;

namespace FreshHarvestMarket.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _http;
        private readonly IRepository<Produce> _produceRepo;
        private readonly IRepository<Discount> _discountRepo;

        public CartService(IHttpContextAccessor http, IRepository<Produce> produceRepo, IRepository<Discount> discountRepo)
        {
            _http = http;
            _produceRepo = produceRepo;
            _discountRepo = discountRepo;
        }

        private ISession Session => _http.HttpContext!.Session;
        private const string Key = "CART";

        // -------------------------
        // GET CART
        // -------------------------
        public List<CartItem> GetCart()
        {
            var json = Session.GetString(Key);
            return json == null ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        // -------------------------
        // SAVE CART
        // -------------------------
        public void SaveCart(List<CartItem> cart)
        {
            Session.SetString(Key, JsonSerializer.Serialize(cart));
        }

        // -------------------------
        // ADD ITEM 
        // -------------------------
        public void AddItem(CartItem item)
        {
            var cart = GetCart();
            var product = _produceRepo.GetAll()
                .FirstOrDefault(p => p.ProduceId == item.ProduceId);

            if (product == null) return;

            // Apply discount if user is logged in
            if (_http.HttpContext!.User.Identity!.IsAuthenticated)
            {
                var discount = _discountRepo.GetAll()
                    .FirstOrDefault(d => d.ProduceId == product.ProduceId);

                // If no discount, set DiscountAmount to 0, otherwise apply the discount
                item.DiscountAmount = discount?.DiscountAmount ?? 0;
            }

            var existing = cart.FirstOrDefault(x => x.ProduceId == item.ProduceId);

            if (existing != null)
            {
                int newQty = existing.Quantity + item.Quantity;
                existing.Quantity = Math.Min(newQty, product.InventoryTotal);
            }
            else
            {
                item.Quantity = Math.Min(item.Quantity, product.InventoryTotal);
                cart.Add(item);
            }

            SaveCart(cart);
        }

        // -------------------------
        // REMOVE ITEM
        // -------------------------
        public void RemoveItem(int id)
        {
            var cart = GetCart();
            cart.RemoveAll(x => x.ProduceId == id);
            SaveCart(cart);
        }

        public decimal GetTotal()
        {
            var cart = GetCart();

            return cart.Sum(x =>
                (x.DiscountAmount.HasValue && x.DiscountAmount > 0)
                    ? x.DiscountedPrice * x.Quantity
                    : x.Price * x.Quantity
            );
        }

        public void ClearCart()
        {
            Session.Remove(Key);
        }

        // -------------------------
        // UPDATE QUANTITY Of PRODUCE
        // -------------------------
        public void UpdateQuantity(int produceId, int quantity)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProduceId == produceId);
            if (item == null) return;

            var product = _produceRepo.GetAll()
                .FirstOrDefault(p => p.ProduceId == produceId);

            if (product == null)
            {
                cart.Remove(item);
                SaveCart(cart);
                return;
            }

            // HARD CLAMP to real inventory
            quantity = Math.Clamp(quantity, 1, product.InventoryTotal);

            item.Quantity = quantity;

            SaveCart(cart);
        }
    }
}