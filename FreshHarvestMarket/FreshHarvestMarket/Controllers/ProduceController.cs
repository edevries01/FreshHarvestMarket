using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class ProduceController : Controller
    {
        private FreshMarketContext _context;

        public ProduceController(FreshMarketContext context)
        {
            _context = context;
        }

        //Retrieves a list of produce from the database, filtered by catgetory (optional)
        public IActionResult Index(string category = "All")
        {
            var produceQuery = _context.Produce.AsQueryable();

            if (category != "All")
            {
                produceQuery = produceQuery.Where(p => p.ProduceCategory == category);
            }

            var viewModel = new BrowseProduceViewModel
            {
                ProduceItems = produceQuery.ToList(),
                SelectedCategory = category,
                Categories = _context.Produce
                    .Select(p => p.ProduceCategory)
                    .Distinct()
                    .ToList()
            };
            return View(viewModel);

        }

        //Finds produce item by ID. Returns NotFound if item not found.
        public IActionResult Details(int id)
        {
            var item = _context.Produce.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        //Add Favorites
        [HttpPost]
        public IActionResult AddFavorite(int produceId)
        {
            var existing = _context.Favorites.FirstOrDefault(f => f.ProduceId == produceId);

            if (existing == null)
            {
                var favorite = new Favorite { ProduceId = produceId };

                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        //Get Produce/Favorites
        public IActionResult Favorites()
        {
            // Join Favorites table with Produce table to get full details
            var favoriteItems = _context.Favorites
                .Join(_context.Produce,
                f => f.ProduceId,
                p => p.ProduceId,
                (f, p) => p).ToList();

            var viewModel = new FavoritesViewModel
            {
                FavoriteItems = favoriteItems
            };

            return View(viewModel);
        }

        //Post Produce/DeleteFavorite
        [HttpPost]
        public IActionResult DeleteFavorite(int produceId)
        {
            //Find record in Favorites table
            var favorite = _context.Favorites.FirstOrDefault(f => f.ProduceId == produceId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            //Redirect back to Favorites view to refresh list
            return RedirectToAction("Favorites");
        }

        //Admin actions
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Produce item)
        {
            if (ModelState.IsValid)
            {
                _context.Produce.Add(item);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _context.Produce.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Produce item)
        {
            if (ModelState.IsValid)
            {
                _context.Produce.Update(item);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Produce.Find();
            if (item != null)
            {
                _context.Produce.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
