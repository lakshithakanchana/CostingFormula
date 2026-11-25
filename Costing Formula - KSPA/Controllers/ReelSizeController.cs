using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class ReelSizeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReelSizeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            IEnumerable<ReelSize> reelSizeList = _db.ReelSizes;
            return View(reelSizeList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReelSize reelSize)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (reelSize == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.ReelSizes.Add(reelSize);
                ViewBag.SuccessMessage = "Reel size created successfully!";
                _db.SaveChanges();
            } else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            return View(reelSize);
        }

        public IActionResult Update(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var reelSize = _db.ReelSizes.Find(id);
            if (reelSize == null)
            {
                return NotFound();
            }
            return View(reelSize);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ReelSize rellSize)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (rellSize == null || rellSize.ReelSizeId == 0)
            {
                return NotFound();
            }
            //if (_db.Colors.Find(color.ColorId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.ReelSizes.Update(rellSize);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Reel size updated successfully!";
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(rellSize);
        }
    }
}