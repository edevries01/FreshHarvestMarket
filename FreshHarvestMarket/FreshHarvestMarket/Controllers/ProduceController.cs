using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class ProduceController : Controller
    {
        private IRepository<Produce> _produceRepository;
        private IRepository<Favorite> _favoriteRepository;

        public ProduceController(IRepository<Produce> produceRepository, IRepository<Favorite> favoriteRepository)
        {
            _produceRepository = produceRepository;
            _favoriteRepository = favoriteRepository;
        }

        //Retrieves a list of produce from the database, filtered by catgetory (optional)
        public IActionResult Index(string category = "All")
        {
            var produceQuery = _produceRepository.GetAll().AsQueryable();

            if (category != "All")
            {
                produceQuery = produceQuery.Where(p => p.ProduceCategory == category);
            }

            var viewModel = new BrowseProduceViewModel
            {
                ProduceItems = produceQuery.ToList(),
                SelectedCategory = category,
                Categories = _produceRepository.GetAll()
                    .Select(p => p.ProduceCategory)
                    .Distinct()
                    .ToList(),

                 FavoriteIds = _favoriteRepository.GetAll()
                    .Select(f => f.ProduceId)
                    .ToList()
            };

            

            return View(viewModel);

        }

        //Finds produce item by ID. Returns NotFound if item not found.
        public IActionResult Details(int id)
        {
            var item = _produceRepository.GetAll().FirstOrDefault(p => p.ProduceId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        //Add Favorites
        [HttpPost]
        public IActionResult AddFavorite(int produceId)
        {
            var existing = _favoriteRepository.GetAll().FirstOrDefault(f => f.ProduceId == produceId);

            if (existing == null)
            {
                var favorite = new Favorite { ProduceId = produceId };

                _favoriteRepository.Insert(favorite);
                _favoriteRepository.Save();
            }

            return RedirectToAction("Index");

        }

        //Get Produce/Favorites
        public IActionResult Favorites()
        {
            // Join Favorites table with Produce table to get full details
            var favorites = _favoriteRepository.GetAll().ToList();

                var favoriteItems = _produceRepository.GetAll()
                    .ToList()
                    .Where(p => favorites.Any(f => f.ProduceId == p.ProduceId))
                    .ToList();


            var viewModel = new FavoritesViewModel
            {
                FavoriteItems = favoriteItems,

                FavoriteIds = favorites.Select(f => f.ProduceId).ToList()
            };

            return View(viewModel);
        }

        //Post Produce/DeleteFavorite
        [HttpPost]
        public IActionResult DeleteFavorite(int produceId)
        {
            //Find record in Favorites table
            var favorite = _favoriteRepository.GetAll().FirstOrDefault(f => f.ProduceId == produceId);

            if (favorite != null)
            {
                _favoriteRepository.Delete(favorite);
                _favoriteRepository.Save();
            }

            //Redirect back to Favorites view to refresh list
            return RedirectToAction("Favorites");
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult ManageProduce()
        {
            var items = _produceRepository.GetAll().ToList();
            return View(items);
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
                _produceRepository.Insert(item);
                _produceRepository.Save();
                return RedirectToAction(nameof(ManageProduce));
            }

            return View(item);
        }


        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _produceRepository.GetAll().FirstOrDefault(p=>p.ProduceId==id);
            if (item == null) return NotFound();
            return View(item);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Produce item)
        {
            if (ModelState.IsValid)
            {
                _produceRepository.Update(item);
                _produceRepository.Save();
                return RedirectToAction(nameof(ManageProduce));
            }
            return View(item);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateInventory(int produceId, int inventoryTotal)
        {
            var item = _produceRepository.GetAll().FirstOrDefault(p => p.ProduceId == produceId);

            if (item != null)
            {
                item.InventoryTotal = inventoryTotal;

                _produceRepository.Update(item);
                _produceRepository.Save();
            }

            return RedirectToAction(nameof(ManageProduce));
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            //var item = _context.Produce.Find(); I left this here since I changed the logic, missing id?
            var item = _produceRepository.GetAll().FirstOrDefault(p=>p.ProduceId==id);
            if (item != null)
            {
                _produceRepository.Delete(item);
                _produceRepository.Save();
            }
            return RedirectToAction(nameof(ManageProduce));
        }
    }
}
