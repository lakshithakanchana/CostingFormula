using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Controllers
{
    public class PolyMaterialController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PolyMaterialController(ApplicationDbContext db)
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

            IEnumerable<PolyMaterial> polyMaterialList = _db.PolyMaterials;
            return View(polyMaterialList);
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
        public IActionResult Create(PolyMaterial polyMaterial)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (polyMaterial == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.PolyMaterials.Add(polyMaterial);
                ViewBag.SuccessMessage = "Poly-Material added successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(polyMaterial);
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
            var polyMaterial = _db.PolyMaterials.Find(id);
            if (polyMaterial == null)
            {
                return NotFound();
            }
            return View(polyMaterial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PolyMaterial polyMaterial)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            int id = polyMaterial.PolyMaterialId;
            if (id == 0)
            {
                return NotFound();
            }
            //if (_db.PolyMaterials.Find(id) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.PolyMaterials.Update(polyMaterial);
                TempData["SuccessMessage"] = "Poly-Material updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(polyMaterial);
        }

    }
}
