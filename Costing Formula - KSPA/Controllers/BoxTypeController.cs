using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Models.JsonModels;
using Costing_Formula___KSPA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Costing_Formula___KSPA.Controllers
{
    public class BoxTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoxTypeController(ApplicationDbContext db)
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

            IEnumerable<BoxType> boxTypeList = _db.BoxTypes;

            return View(boxTypeList);
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
        public IActionResult Create(BoxType boxType)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (boxType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.BoxTypes.Add(boxType);
                ViewBag.SuccessMessage = "Box type created successfully!";
                _db.SaveChanges();
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            return View(boxType);
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
            var boxType = _db.BoxTypes.Find(id);
            if (boxType == null)
            {
                return NotFound();
            }
            return View(boxType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BoxType boxType)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (boxType == null || boxType.BoxTypeId == 0)
            {
                return NotFound();
            }
            //if (_db.Cylinders.Find(bagTypeVM.BagType.BagTypeId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.BoxTypes.Update(boxType);
                TempData["SuccessMessage"] = "Box type updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                return View(boxType);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<BoxType> boxTypeList = new List<BoxType>();
            boxTypeList.Add(new BoxType()
            {
                BoxTypeCode = "PlaceHolder",
                BoxTypeDesc = "PlaceHolder",
                NoOfPlys = 1,
                ESheetLengthFormula = "PlaceHolder",
                ESheetLengthFormulaIfSLExceeds = "PlaceHolder",
                ESheetWidthFormula = "PlaceHolder",
                ISheetLengthFormula = "PlaceHolder",
                ISheetLengthFormulaIfSLExceeds = "PlaceHolder",
                ISheetWidthFormula = "PlaceHolder",
                Laminate = false,
                DieCut = false,
                DesignBox = false,
                Board = false,
                IsActive = false,
            }); // This is a little hack
            return View(boxTypeList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<BoxType> boxTypeList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (boxTypeList == null)
            {
                return View(boxTypeList);
            }

            if (ModelState.IsValid)
            {
                _db.BoxTypes.AddRange(boxTypeList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(boxTypeList);
        }
    }
}
