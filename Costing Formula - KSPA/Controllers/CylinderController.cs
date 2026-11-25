using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class CylinderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CylinderController(ApplicationDbContext db)
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

            IEnumerable<Cylinder> cylinderList = _db.Cylinders;
            return View(cylinderList);
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
        public IActionResult Create(Cylinder cylinder)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (cylinder == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Cylinders.Add(cylinder);
                ViewBag.SuccessMessage = "Cylinder created successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(cylinder);
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
            var cylinder = _db.Cylinders.Find(id);
            if(cylinder == null)
            {
                return NotFound();
            }
            return View(cylinder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Cylinder cylinder)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            int id = cylinder.CylinderId;
            if (id == 0)
            {
                return NotFound();
            }
            //if (_db.Cylinders.Find(id) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.Cylinders.Update(cylinder);
                TempData["SuccessMessage"] = "Cylinder updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            } else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(cylinder);
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<Cylinder> cylinderList = new List<Cylinder>();
            cylinderList.Add(new Cylinder()
            {
                CylinderCode = "PH",
                CylinderDesc = "PH",
                NoOfColors = 0,
                CylinderSizeCm = 0,
                NoOfPieces = 0,
                IsActive = false
            }); // This is a little hack
            return View(cylinderList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<Cylinder> cylinderList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (cylinderList == null)
            {
                return View(cylinderList);
            }

            if (ModelState.IsValid)
            {
                _db.Cylinders.AddRange(cylinderList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(cylinderList);
        }
    }
}
