using Costing_Formula___KSPA.Data;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RouteController(ApplicationDbContext db)
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

            IEnumerable<Models.Route> routeList = _db.Routes;
            return View(routeList);
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
        public IActionResult Create(Models.Route route)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (route == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Routes.Add(route);
                ViewBag.SuccessMessage = "Route created successfully!";
                _db.SaveChanges();
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            return View(route);
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
            var route = _db.Routes.Find(id);
            if (route == null)
            {
                return NotFound();
            }
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Models.Route route)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (route == null || route.RouteId == 0)
            {
                return NotFound();
            }
            //if (_db.Colors.Find(color.ColorId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.Routes.Update(route);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Route updated successfully!";
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(route);
        }
    }
}
