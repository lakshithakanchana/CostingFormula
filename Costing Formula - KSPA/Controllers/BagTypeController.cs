using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Models.JsonModels;
using Costing_Formula___KSPA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Costing_Formula___KSPA.Controllers
{
    public class BagTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BagTypeController(ApplicationDbContext db)
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

            IEnumerable<BagType> bagTypeList = _db.BagTypes;

            foreach(var bagType in bagTypeList)
            {
                bagType.ProductFinishingType = _db.ProductFinishingTypes.FirstOrDefault(
                        record => record.ProductFinTyId == bagType.ProductFinishingTypeId
                    );
            }

            return View(bagTypeList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            // Getting View Model
            BagTypeVM bagTypeVM = new BagTypeVM()
            {
                BagType = new BagType(),
                ProductFinishingTypeDropDown = _db.ProductFinishingTypes.Select(item => new SelectListItem
                {
                    Text = item.ProductFinishingTypeDesc,
                    Value = item.ProductFinTyId.ToString()
                })
            };
            return View(bagTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BagTypeVM bagTypeVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (bagTypeVM == null || bagTypeVM.BagType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.BagTypes.Add(bagTypeVM.BagType);
                ViewBag.SuccessMessage = "Bag type created successfully!";
                _db.SaveChanges();
            } else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            // To send View(completeObject). Otherwise selectLists will not fill
            bagTypeVM.ProductFinishingTypeDropDown = _db.ProductFinishingTypes.Select(item => new SelectListItem
            {
                Text = item.ProductFinishingTypeDesc,
                Value = item.ProductFinTyId.ToString()
            });

            return View(bagTypeVM);
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
            var bagType = _db.BagTypes.Find(id);
            if (bagType == null)
            {
                return NotFound();
            }

            var bagTypeVM = new BagTypeVM()
            {
                BagType = bagType,
                ProductFinishingTypeDropDown = _db.ProductFinishingTypes.Select(item => new SelectListItem
                {
                    Text = item.ProductFinishingTypeDesc,
                    Value = item.ProductFinTyId.ToString()
                })
            };
            return View(bagTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BagTypeVM bagTypeVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (bagTypeVM == null || bagTypeVM.BagType == null || bagTypeVM.BagType.BagTypeId == 0)
            {
                return NotFound();
            }
            //if (_db.Cylinders.Find(bagTypeVM.BagType.BagTypeId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.BagTypes.Update(bagTypeVM.BagType);
                TempData["SuccessMessage"] = "Bag type updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                bagTypeVM.ProductFinishingTypeDropDown = _db.ProductFinishingTypes.Select(item => new SelectListItem
                {
                    Text = item.ProductFinishingTypeDesc,
                    Value = item.ProductFinTyId.ToString()
                });
                return View(bagTypeVM);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<BagTypeVM> bagTypeVMList = new List<BagTypeVM>();
            bagTypeVMList.Add(new BagTypeVM()
            {
                BagType = new BagType()
                {
                    BagTypeCode = "PlH",
                    ProductFinishingTypeId = 0,
                    BagTypeDesc = "PlH",
                    FormulaToCalcSArea = "PlH",
                    PrintByWidthOrLength = false,
                    SheetSizeFormula = "PlH",
                    Printed = false,
                    SideSeal = false,
                    SideGusset = false,
                    HangerAttached = false,
                    Flap = false,
                    HangerBag = false,
                    BottomGusset = false,
                    HandleCut = false,
                    BottomSeal = false,
                    PunchHole = false,
                    TopSeal = false,
                    Zipper = false
                },
                ProductFinishingTypeDropDown = _db.ProductFinishingTypes.Select(item => new SelectListItem
                {
                    Text = item.ProductFinishingTypeDesc,
                    Value = item.ProductFinTyId.ToString()
                })
            }); // This is a little hack

            var productFinTypeList = _db.ProductFinishingTypes
                .Select(item => new IdTextJsonM
                {
                    Id = item.ProductFinTyId,
                    Text = item.ProductFinishingTypeDesc
                }).ToList<IdTextJsonM>();
            ViewBag.productFinTypesJsonStr = JsonSerializer.Serialize(productFinTypeList);

            return View(bagTypeVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<BagTypeVM> bagTypeVMList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");
            if (bagTypeVMList == null)
            {
                return View(bagTypeVMList);
            }

            List<BagType> bagTypeList = new List<BagType>();
            foreach (var bagTypeVM in bagTypeVMList)
            {
                bagTypeList.Add(bagTypeVM.BagType);
            }

            if (bagTypeList.Count == 0)
            {
                return View(bagTypeVMList);
            }

            if (ModelState.IsValid)
            {
                _db.BagTypes.AddRange(bagTypeList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                return View(bagTypeVMList);
            }
                
        }
    }
}
