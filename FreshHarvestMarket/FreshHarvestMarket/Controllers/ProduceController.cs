/*
 * ProduceController.cs
 * FreshHarvestMarket
 *
 * This controller manages all interactions related to produce items
 * within the Fresh Harvest Market application.
 *
 * It handles:
 * - Displaying available produce items with optional category filtering
 * - Viewing detailed information for a specific produce item
 * - Managing user favorites (add, view, remove)
 * - Admin functionality for managing produce (create, edit, delete, update inventory)
 *
 * The controller uses repository services to interact with the database,
 * promoting separation of concerns & easier testing.
 *
 * It also utilizes view models to organize & pass structured data
 * between the controller & views.
 *
 * Some actions are intended for administrative use & may be secured
 * with role-based authorization.
 */

using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Controllers
{
    public class ProduceController : Controller
    {
        private IRepository<Produce> _produceRepository;
        private IRepository<Favorite> _favoriteRepository;
        private readonly UserManager<User> _userManager;

        public ProduceController(
            IRepository<Produce> produceRepository, 
            IRepository<Favorite> favoriteRepository,
            UserManager<User> userManager)
        {
            _produceRepository = produceRepository;
            _favoriteRepository = favoriteRepository;
            _userManager = userManager;
        }

        //Retrieves a list of produce from the database, filtered by catgetory (optional)
        public IActionResult Index(string category = "All")
        {
            var produceQuery = _produceRepository.GetAll()
                .Include(p => p.Discount)
                .AsQueryable();

            if (category != "All")
            {
                produceQuery = produceQuery.Where(p => p.ProduceCategory == category);
            }

            var userId = _userManager.GetUserId(User);

            var viewModel = new BrowseProduceViewModel
            {
                ProduceItems = produceQuery.ToList(),
                SelectedCategory = category,
                Categories = _produceRepository.GetAll()
                    .Select(p => p.ProduceCategory)
                    .Distinct()
                    .ToList(),

                 FavoriteIds = _favoriteRepository.GetAll()
                    .Where(f => f.UserId == userId)
                    .Select(f => f.ProduceId)
                    .ToList()
            };

            

            return View(viewModel);

        }

        //Finds produce item by ID. Returns NotFound if item not found.
        public IActionResult Details(int id)
        {
            var item = _produceRepository.GetAll()
                .Include(p => p.Discount)
                .FirstOrDefault(p => p.ProduceId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        //Add Favorites
        [Authorize]
        [HttpPost]
        public IActionResult AddFavorite(int produceId)
        {
            var userId = _userManager.GetUserId(User);

            var existing = _favoriteRepository.GetAll()
                .FirstOrDefault(f => f.ProduceId == produceId && f.UserId == userId);

            if (existing == null)
            {
                var favorite = new Favorite 
                { 
                    ProduceId = produceId,
                    UserId = userId
                };

                _favoriteRepository.Insert(favorite);
                _favoriteRepository.Save();
            }

            return RedirectToAction("Index");

        }

        //Get Produce/Favorites
        [Authorize]
        public IActionResult Favorites()
        {
            var userId = _userManager.GetUserId(User);

            // Join Favorites table with Produce table to get full details
            var favorites = _favoriteRepository.GetAll()
                .Where(f => f.UserId == userId)
                .ToList();

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
        [Authorize]
        [HttpPost]
        public IActionResult DeleteFavorite(int produceId)
        {
            var userId = _userManager.GetUserId(User);

            //Find record in Favorites table
            var favorite = _favoriteRepository.GetAll()
                .FirstOrDefault(f => f.ProduceId == produceId && f.UserId == userId);

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
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            // Check for duplicate name (case-insensitive)
            bool exists = _produceRepository.GetAll()
                .Any(p => p.ProduceName.ToLower() == item.ProduceName.ToLower());

            if (exists)
            {
                ModelState.AddModelError("ProduceName", "A product with this name already exists.");
                return View(item);
            }

            _produceRepository.Insert(item);
            _produceRepository.Save();

            return RedirectToAction(nameof(ManageProduce));
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
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            // Check for duplicate name (excluding the current item)
            bool exists = _produceRepository.GetAll()
                .Any(p => p.ProduceId != item.ProduceId &&
                          p.ProduceName.Trim().ToLower() == item.ProduceName.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("ProduceName", "A product with this name already exists.");
                return View(item);
            }

            _produceRepository.Update(item);
            _produceRepository.Save();

            return RedirectToAction(nameof(ManageProduce));
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
