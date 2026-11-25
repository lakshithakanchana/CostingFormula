using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Costing_Formula___KSPA.Controllers
{
    public class PolyCostingController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PolyCostingController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCostingsData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            // 1. Role Logic (Same as your Index method)
            IQueryable<PolyCosting> query = _db.PolyCostings
                .Include(q => q.Customer).ThenInclude(q => q.Currency)
                .Include(q => q.Customer).ThenInclude(q => q.SRep)
                .Include(q => q.BagType)
                .Include(q => q.PolyCombination).ThenInclude(pc => pc.PolyMaterial)
                .Include(q => q.MeasurementUom);

            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
            {
                var userName = User.Identity.Name.ToLower();
                query = query.Where(fields => (fields.CreatedUser ?? "null").ToLower() == userName || fields.CreatedUser == null);
            }

            // 2. Searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                query = query.Where(m =>
                    m.PolyCostingId.ToString().Contains(searchValue) ||
                    (m.Customer != null && m.Customer.CustomerShortCode != null && m.Customer.CustomerShortCode.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.CustomerName != null && m.Customer.CustomerName.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.Currency != null && m.Customer.Currency.CurrencyCode != null && m.Customer.Currency.CurrencyCode.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.SRep != null && m.Customer.SRep.SrocName != null && m.Customer.SRep.SrocName.ToLower().Contains(searchValue)) ||
                    (m.BagType != null && m.BagType.BagTypeDesc != null && m.BagType.BagTypeDesc.ToLower().Contains(searchValue)) ||
                    (m.PolyCombination != null && m.PolyCombination.PolyCombDesc != null && m.PolyCombination.PolyCombDesc.ToLower().Contains(searchValue))
                );
            }

            int recordsTotal = await query.CountAsync();

            // 3. Sorting (Preventing NotMapped Translation Errors!)
            if (!string.IsNullOrEmpty(sortColumnIndex) && !string.IsNullOrEmpty(sortColumnDir))
            {
                bool isDesc = sortColumnDir == "desc";
                query = sortColumnIndex switch
                {
                    "0" => isDesc ? query.OrderByDescending(c => c.PolyCostingId) : query.OrderBy(c => c.PolyCostingId),
                    "1" => isDesc ? query.OrderByDescending(c => c.PolyCostingId) : query.OrderBy(c => c.PolyCostingId), // Code is NotMapped
                    "2" => isDesc ? query.OrderByDescending(c => c.DateCreated) : query.OrderBy(c => c.DateCreated),
                    "3" => isDesc ? query.OrderByDescending(c => c.Customer.CustomerName) : query.OrderBy(c => c.Customer.CustomerName),
                    "4" => isDesc ? query.OrderByDescending(c => c.Customer.CustomerShortCode) : query.OrderBy(c => c.Customer.CustomerShortCode),
                    "5" => isDesc ? query.OrderByDescending(c => c.Guage).ThenByDescending(c => c.Width) : query.OrderBy(c => c.Guage).ThenBy(c => c.Width), // Sort by underlying dimensions
                    "6" => isDesc ? query.OrderByDescending(c => c.OrderQty) : query.OrderBy(c => c.OrderQty),
                    "7" => isDesc ? query.OrderByDescending(c => c.Weight) : query.OrderBy(c => c.Weight),
                    "8" => isDesc ? query.OrderByDescending(c => c.Customer.Currency.CurrencyCode) : query.OrderBy(c => c.Customer.Currency.CurrencyCode),
                    "9" => isDesc ? query.OrderByDescending(c => c.PerUnitAdjustableUnitPrice) : query.OrderBy(c => c.PerUnitAdjustableUnitPrice),
                    "11" => isDesc ? query.OrderByDescending(c => c.PerUnitAdjustableUnitPrice * c.OrderQty) : query.OrderBy(c => c.PerUnitAdjustableUnitPrice * c.OrderQty),
                    "13" => isDesc ? query.OrderByDescending(c => c.Customer.SRep.SrocName) : query.OrderBy(c => c.Customer.SRep.SrocName),
                    _ => isDesc ? query.OrderByDescending(c => c.PolyCostingId) : query.OrderBy(c => c.PolyCostingId)
                };
            }

            // 4. Pagination
            var pagedData = await query.Skip(skip).Take(pageSize).ToListAsync();

            // 5. Fetch Exchange Rates in Memory (only for the paged subset)
            foreach (var polyCosting in pagedData)
            {
                polyCosting.SelectedExchangeRate = _db.ExchangeRates
                    .Where(recordE => recordE.CurrencyId == polyCosting.Customer.CurrencyId && recordE.EffectiveFrom <= polyCosting.DateCreated)
                    .OrderByDescending(fields => fields.EffectiveFrom)
                    .FirstOrDefault()?.RateInLKR ?? 0;
            }

            // 6. Format JSON Response
            var data = pagedData.Select(c => new
            {
                id = c.PolyCostingId,
                quotationNo = c.PolyCostingCode, // NotMapped getter works safely in memory here
                docDate = c.DateCreated.ToString("yyyy/MM/dd hh:mm:ss"),
                customerName = c.Customer?.CustomerName ?? "",
                customerCode = c.Customer?.CustomerShortCode ?? "",
                itemName = c.ItemName ?? "", // NotMapped getter works safely in memory here
                orderQty = c.OrderQty.ToString("n0"),
                weight = c.Weight.ToString("n2"),
                currencyCode = c.Customer?.Currency?.CurrencyCode ?? "",
                rateNative = c.PerUnitAdjustableUnitPrice.ToString("n2"),
                rateLkr = (c.PerUnitAdjustableUnitPrice * (c.SelectedExchangeRate ?? 0)).ToString("n2"),
                salesPriceNative = (c.PerUnitAdjustableUnitPrice * c.OrderQty).ToString("n2"),
                salesPriceLkr = (c.PerUnitAdjustableUnitPrice * c.OrderQty * (c.SelectedExchangeRate ?? 0)).ToString("n2"),
                salesRep = c.Customer?.SRep?.SrocName ?? ""
            });

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        // Create by cloning
        public IActionResult Create(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            PolyCosting? polyCosting;

            if (id == null || id == 0)
            {
                polyCosting = new PolyCosting()
                {
                    DateCreated = DateTime.Now
                };
            }
            else
            {
                polyCosting = _db.PolyCostings.Find(id);
                if (polyCosting == null)
                {
                    return NotFound();
                }
                polyCosting.PolyCostingId = 0;
            }

            int mesurementIdOfOrders = _db.Measurements.First(m =>
                m.MeasurementDesc == "ORDERS"
            ).MeasurementId;

            int mesurementIdOfLength = _db.Measurements.First(m =>
                m.MeasurementDesc == "LENGTH"
            ).MeasurementId;

            // Getting View Model
            PolyCostingVM polyCostingVM = new PolyCostingVM()
            {
                PolyCosting = polyCosting,
                CompanyDropDown = _db.Companies
                .Where(field => 
                    field.IsActive
                ).OrderBy(field =>
                    field.CompanyName
                ).Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                }),
                CustomerDropDown = _db.Customers
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CustomerName
                ).Select(item => new SelectListItem
                {
                    Text = item.CustomerName,
                    Value = item.CustomerId.ToString()
                }),
                ColorDropDown = _db.Colors
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.ColorName
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PolyCombinationDropDown = _db.PolyCombinations
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.PolyCombDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.PolyCombDesc + " (" + item.PolyCombCode + ")",
                    Value = item.PolyCombId.ToString()
                }),
                PolyMaterialDropDown = _db.PolyMaterials
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.PolyMatCode
                ).Select(item => new SelectListItem
                {
                    Text = item.PolyMatCode,
                    Value = item.PolyMaterialId.ToString()
                }),
                BagTypeDropDown = _db.BagTypes
                .Where(field =>
                    field.Isactive
                ).OrderBy(field =>
                    field.BagTypeCode
                ).Select(item => new SelectListItem
                {
                    Text = item.BagTypeDesc + " (" + item.BagTypeCode + ")",
                    Value = item.BagTypeId.ToString()
                }),
                OrderUomDropDown = _db.Uoms
                .Where(uom => //
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfOrders
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                }),
                LengthUomDropDown = _db.Uoms.Where(uom => //
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfLength
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                }),
                PrintTypeDropDown = _db.PrintTypes
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.PrintTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.PrintTypeDesc,
                    Value = item.PrintTypeId.ToString()
                }),
                ShipmentTermDropDown = _db.ShipmentTerms
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.ShipmentTermName
                ).Select(item => new SelectListItem
                {
                    Text = item.ShipmentTermName,
                    Value = item.ShipmentTermId.ToString()
                }),
                CylinderCodeDropDown = _db.Cylinders
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CylinderCode
                ).Select(item => new SelectListItem
                {
                    Text = item.CylinderCode,
                    Value = item.CylinderId.ToString()
                })
            };

            List<CustomerJM> customerJMList = _db.Customers
            .Where(fields =>
                fields.IsActive == true
            ).Select(fields => new CustomerJM
            {
                Id = fields.CustomerId,
                CustomerTypeDesc =
                    (
                        _db.CustomerTypes.FirstOrDefault(row =>
                            row.CustomerTypeId == fields.CustomerTypeId
                        )
                        ?? new CustomerType { CustomerTypeDesc = "" }
                    ).CustomerTypeDesc,
                CurrencyCode =
                    (
                        _db.Currencies.FirstOrDefault(row =>
                            row.CurrencyId == fields.CurrencyId
                        )
                        ?? new Currency { CurrencyCode = "" }
                    ).CurrencyCode,
                RateInLKR =
                    (
                        _db.ExchangeRates.Where(recordE =>
                            recordE.CurrencyId == fields.CurrencyId
                            &&
                            recordE.IsActive
                            &&
                            recordE.EffectiveFrom <= DateTime.Now
                        ).OrderByDescending(fields =>
                            fields.EffectiveFrom
                        ).FirstOrDefault()
                        ?? new ExchangeRate { RateInLKR = 0d }
                    ).RateInLKR,
                CustomerAddress = fields.CustomerAddress ?? ""
            }
            ).ToList();
            ViewBag.customersJsonStr = JsonSerializer.Serialize(customerJMList);

            List<PolyCombinationJM> polyCombinationJMList = _db.PolyCombinations
            .Where(fieldsA =>
                fieldsA.IsActive
            ).OrderBy(fieldsA =>
                fieldsA.PolyCombCode
            ).Select(fieldsB => new PolyCombinationJM
            {
                Id = fieldsB.PolyCombId,
                ColorId = fieldsB.ColorId,
                PolyMaterialId = fieldsB.PolyMaterialId,
                PolyCombCode = fieldsB.PolyCombCode,
                PolyCombDesc = fieldsB.PolyCombDesc,
                PolyCombItemCatBridges = _db.PolyCombItemCatBridges
                    .Where(fieldsC =>
                        fieldsC.PolyCombId == fieldsB.PolyCombId
                    ).Select(fields2 => new PolyCombItemCatBridgeJM
                    {
                        Id = fields2.ItemCatId,
                        Percentage = fields2.Percentage,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(fields3 =>
                                    fields3.IsActive
                                    && fields3.ItemCatId == fields2.ItemCatId
                                    ).OrderByDescending(fields5 =>
                                    fields5.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitPrice = 0d })
                                ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(fields4 =>
                                    fields4.IsActive
                                    && fields4.ItemCatId == fields2.ItemCatId).OrderByDescending(fields6 =>
                                    fields6.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitDuty = 0d })
                                ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.polyCombinationJsonStr = JsonSerializer.Serialize(polyCombinationJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.BlockCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "BC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.OtherCosts = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            List<BagTypeJM> bagTypeJMList = _db.BagTypes
                .Where(fields =>
                    fields.Isactive == true
                ).Select(fields => new BagTypeJM
                {
                    Id = fields.BagTypeId,
                    BagTypeDesc = fields.BagTypeDesc,
                    ProductFinishingTypeDesc =
                    (
                        _db.ProductFinishingTypes.FirstOrDefault(fields2 =>
                            fields2.ProductFinTyId == fields.ProductFinishingTypeId
                        )
                        ?? new ProductFinishingType { ProductFinishingTypeDesc = "" }
                    ).ProductFinishingTypeDesc,
                    PrintByWidthOrLength = fields.PrintByWidthOrLength,
                    FormulaToCalcSArea = fields.FormulaToCalcSArea,
                    SheetSizeFormula = fields.SheetSizeFormula
                }).ToList();
            ViewBag.bagTypeJsonStr = JsonSerializer.Serialize(bagTypeJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<CylinderJM> cylinderJMList = _db.Cylinders
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new CylinderJM
                {
                    Id = fields.CylinderId,
                    CylinderCode = fields.CylinderCode,
                    CylinderSizeCm = fields.CylinderSizeCm,
                    NoOfColors = fields.NoOfColors
                }).ToList();
            ViewBag.cylinderJsonStr = JsonSerializer.Serialize(cylinderJMList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(field =>
                    field.IsActive
                    && field.IsPolyComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            return View(polyCostingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PolyCostingVM polyCostingVM)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (polyCostingVM == null || polyCostingVM.PolyCosting == null)
            {
                return NotFound();
            }

            polyCostingVM.PolyCosting.DateCreated = DateTime.Now;
            polyCostingVM.PolyCosting.CreatedUser = User?.Identity?.Name;

            if (ModelState.IsValid)
            {
                var company = _db.Companies
                    .Find(polyCostingVM.PolyCosting.CompanyId);
                if (company!.CompanyName.Contains("PRINTING", StringComparison.OrdinalIgnoreCase))
                {
                    var customer = _db.Customers
                        .Include(c => c.CustomerType)
                        .FirstOrDefault(c =>
                            c.CustomerId == polyCostingVM.PolyCosting.CustomerId
                        );
                    var isLocal = customer!.CustomerType!
                        .CustomerTypeDesc
                        .Contains("LOCAL", StringComparison.OrdinalIgnoreCase);
                    if (isLocal)
                    {
                        _db.PolyCostings.Add(polyCostingVM.PolyCosting);
                        ViewBag.SuccessMessage = "Quotation created successfully!";
                        _db.SaveChanges();

                        return RedirectToAction("View", new { id = polyCostingVM.PolyCosting.PolyCostingId });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Only LOCAL customers are allowed for this company.";
                    }
                }
                else
                {
                    _db.PolyCostings.Add(polyCostingVM.PolyCosting);
                    ViewBag.SuccessMessage = "Quotation created successfully!";
                    _db.SaveChanges();

                    return RedirectToAction("View", new { id = polyCostingVM.PolyCosting.PolyCostingId });
                }
            } else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            int mesurementIdOfOrders = _db.Measurements.FirstOrDefault(m =>
                m.MeasurementDesc == "ORDERS"
            ).MeasurementId;

            int mesurementIdOfLength = _db.Measurements.FirstOrDefault(m =>
                m.MeasurementDesc == "LENGTH"
            ).MeasurementId;

            // To send View(completeObject). Otherwise selectLists will not fill
            polyCostingVM.CompanyDropDown = _db.Companies
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.CompanyName
            ).Select(item => new SelectListItem
            {
                Text = item.CompanyName,
                Value = item.CompanyId.ToString()
            });
            polyCostingVM.CustomerDropDown = _db.Customers
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.CustomerName
            ).Select(item => new SelectListItem
            {
                Text = item.CustomerName,
                Value = item.CustomerId.ToString()
            });
            polyCostingVM.ColorDropDown = _db.Colors
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.ColorName
            ).Select(item => new SelectListItem
            {
                Text = item.ColorName,
                Value = item.ColorId.ToString()
            });
            polyCostingVM.PolyCombinationDropDown = _db.PolyCombinations
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.PolyCombCode
            ).Select(item => new SelectListItem
            {
                Text = item.PolyCombDesc + " (" + item.PolyCombCode + ")",
                Value = item.PolyCombId.ToString()
            });
            polyCostingVM.PolyMaterialDropDown = _db.PolyMaterials
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.PolyMatCode
            ).Select(item => new SelectListItem
            {
                Text = item.PolyMatCode,
                Value = item.PolyMaterialId.ToString()
            });
            polyCostingVM.BagTypeDropDown = _db.BagTypes
            .Where(field =>
                field.Isactive
            ).OrderBy(field =>
                field.BagTypeCode
            ).Select(item => new SelectListItem
            {
                Text = item.BagTypeDesc + " (" + item.BagTypeCode + ")",
                Value = item.BagTypeId.ToString()
            });
            polyCostingVM.OrderUomDropDown = _db.Uoms.Where(uom =>
                uom.IsActive == true
                &&
                uom.MeasurementId == mesurementIdOfOrders
            ).Select(item => new SelectListItem
            {
                Text = item.UomCode,
                Value = item.UomId.ToString()
            });
            polyCostingVM.LengthUomDropDown = _db.Uoms.Where(uom =>
                uom.IsActive == true
                &&
                uom.MeasurementId == mesurementIdOfLength
            ).Select(item => new SelectListItem
            {
                Text = item.UomCode,
                Value = item.UomId.ToString()
            });
            polyCostingVM.PrintTypeDropDown = _db.PrintTypes
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.PrintTypeDesc
            ).Select(item => new SelectListItem
            {
                Text = item.PrintTypeDesc,
                Value = item.PrintTypeId.ToString()
            });
            polyCostingVM.ShipmentTermDropDown = _db.ShipmentTerms
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.ShipmentTermName
            ).Select(item => new SelectListItem
            {
                Text = item.ShipmentTermName,
                Value = item.ShipmentTermId.ToString()
            });
            polyCostingVM.CylinderCodeDropDown = _db.Cylinders
            .Where(field =>
                field.IsActive
            ).OrderBy(field =>
                field.CylinderCode
            ).Select(item => new SelectListItem
            {
                Text = item.CylinderCode,
                Value = item.CylinderId.ToString()
            });

            List<CustomerJM> customerJMList = _db.Customers
            .Where(fields =>
                fields.IsActive
            ).Select(fields => new CustomerJM
            {
                Id = fields.CustomerId,
                CustomerTypeDesc =
                    (
                        _db.CustomerTypes.FirstOrDefault(row =>
                            row.CustomerTypeId == fields.CustomerTypeId
                        )
                        ?? new CustomerType { CustomerTypeDesc = "" }
                    ).CustomerTypeDesc,
                CurrencyCode =
                    (
                        _db.Currencies.FirstOrDefault(row =>
                            row.CurrencyId == fields.CurrencyId
                        )
                        ?? new Currency { CurrencyCode = "" }
                    ).CurrencyCode,
                RateInLKR =
                    (
                        _db.ExchangeRates.Where(recordE =>
                            recordE.CurrencyId == fields.CurrencyId
                            &&
                            recordE.IsActive
                            &&
                            recordE.EffectiveFrom <= DateTime.Now
                        ).OrderByDescending(fields =>
                            fields.EffectiveFrom
                        ).FirstOrDefault()
                        ?? new ExchangeRate { RateInLKR = 0 }
                    ).RateInLKR,
                CustomerAddress = fields.CustomerAddress ?? ""
            }
            ).ToList();
            ViewBag.customersJsonStr = JsonSerializer.Serialize(customerJMList);

            List<PolyCombinationJM> polyCombinationJMList = _db.PolyCombinations
            .Where(fieldsA =>
                fieldsA.IsActive
            ).OrderBy(fieldsA =>
                fieldsA.PolyCombCode
            ).Select(fieldsB => new PolyCombinationJM
            {
                Id = fieldsB.PolyCombId,
                ColorId = fieldsB.ColorId,
                PolyMaterialId = fieldsB.PolyMaterialId,
                PolyCombCode = fieldsB.PolyCombCode,
                PolyCombDesc = fieldsB.PolyCombDesc,
                PolyCombItemCatBridges = _db.PolyCombItemCatBridges
                    .Where(fieldsC =>
                        fieldsC.PolyCombId == fieldsB.PolyCombId
                    ).Select(fields2 => new PolyCombItemCatBridgeJM
                    {
                        Id = fields2.ItemCatId,
                        Percentage = fields2.Percentage,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(fields3 =>
                                    fields3.IsActive
                                    && fields3.ItemCatId == fields2.ItemCatId
                                    ).OrderByDescending(fields5 =>
                                    fields5.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitPrice = 0d })
                                ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(fields4 =>
                                    fields4.IsActive
                                    && fields4.ItemCatId == fields2.ItemCatId).OrderByDescending(fields6 =>
                                    fields6.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitDuty = 0d })
                                ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.polyCombinationJsonStr = JsonSerializer.Serialize(polyCombinationJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.BlockCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "BC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.OtherCosts = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OC").ItemCatId
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            List<BagTypeJM> bagTypeJMList = _db.BagTypes
                .Where(fields =>
                    fields.Isactive == true
                ).Select(fields => new BagTypeJM
                {
                    Id = fields.BagTypeId,
                    BagTypeDesc = fields.BagTypeDesc,
                    ProductFinishingTypeDesc =
                    (
                        _db.ProductFinishingTypes.FirstOrDefault(fields2 =>
                            fields2.ProductFinTyId == fields.ProductFinishingTypeId
                        )
                        ?? new ProductFinishingType { ProductFinishingTypeDesc = "" }
                    ).ProductFinishingTypeDesc,
                    PrintByWidthOrLength = fields.PrintByWidthOrLength,
                    FormulaToCalcSArea = fields.FormulaToCalcSArea,
                    SheetSizeFormula = fields.SheetSizeFormula
                }).ToList();
            ViewBag.bagTypeJsonStr = JsonSerializer.Serialize(bagTypeJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<CylinderJM> cylinderJMList = _db.Cylinders
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new CylinderJM
                {
                    Id = fields.CylinderId,
                    CylinderCode = fields.CylinderCode,
                    CylinderSizeCm = fields.CylinderSizeCm,
                    NoOfColors = fields.NoOfColors
                }).ToList();
            ViewBag.cylinderJsonStr = JsonSerializer.Serialize(cylinderJMList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Where(field =>
                    field.IsActive
                ).Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(field =>
                    field.IsActive
                    && field.IsPolyComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            // To get item name these must be kept filled: error rectification
            polyCostingVM.PolyCosting.MeasurementUom = _db.Uoms.Find(polyCostingVM.PolyCosting.MeasurementUomId);
            polyCostingVM.PolyCosting.PolyCombination = _db.PolyCombinations.Find(polyCostingVM.PolyCosting.PolyCombId);
            if (polyCostingVM.PolyCosting.PolyCombination != null)
            {
                polyCostingVM.PolyCosting.PolyCombination.PolyMaterial = _db.PolyMaterials.Find(polyCostingVM.PolyCosting.PolyCombination.PolyMaterialId);
                polyCostingVM.PolyCosting.PolyCombination.Color = _db.Colors.Find(polyCostingVM.PolyCosting.PolyCombination.ColorId);
            }
            polyCostingVM.PolyCosting.BagType = _db.BagTypes.Find(polyCostingVM.PolyCosting.BagTypeId);

            return View(polyCostingVM);
        }

        public IActionResult View(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var polyCosting = _db.PolyCostings.Find(id);
            if (polyCosting == null)
            {
                return NotFound();
            }

            int mesurementIdOfOrders = _db.Measurements.FirstOrDefault(m =>
                m.MeasurementDesc == "ORDERS"
            ).MeasurementId;

            int mesurementIdOfLength = _db.Measurements.FirstOrDefault(m =>
                m.MeasurementDesc == "LENGTH"
            ).MeasurementId;

            // Getting View Model
            PolyCostingVM polyCostingVM = new PolyCostingVM()
            {
                PolyCosting = polyCosting,
                CompanyDropDown = _db.Companies //
                .OrderBy(field =>
                    field.CompanyName
                ).Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                }),
                CustomerDropDown = _db.Customers //
                .OrderBy(field =>
                    field.CustomerName
                ).Select(item => new SelectListItem
                {
                    Text = item.CustomerName,
                    Value = item.CustomerId.ToString()
                }),
                ColorDropDown = _db.Colors //
                .OrderBy(field =>
                    field.ColorName
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PolyCombinationDropDown = _db.PolyCombinations //
                .OrderBy(field =>
                    field.PolyCombCode
                ).Select(item => new SelectListItem
                {
                    Text = item.PolyCombDesc + " (" + item.PolyCombCode + ")",
                    Value = item.PolyCombId.ToString()
                }),
                PolyMaterialDropDown = _db.PolyMaterials //
                .OrderBy(field =>
                    field.PolyMatCode
                ).Select(item => new SelectListItem
                {
                    Text = item.PolyMatCode,
                    Value = item.PolyMaterialId.ToString()
                }),
                BagTypeDropDown = _db.BagTypes //
                .OrderBy(field =>
                    field.BagTypeCode
                ).Select(item => new SelectListItem
                {
                    Text = item.BagTypeDesc + " (" + item.BagTypeCode + ")",
                    Value = item.BagTypeId.ToString()
                }),
                OrderUomDropDown = _db.Uoms.Where(uom => //
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfOrders
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                }),
                LengthUomDropDown = _db.Uoms.Where(uom => //
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfLength
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                }),
                PrintTypeDropDown = _db.PrintTypes //
                .OrderBy(field =>
                    field.PrintTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.PrintTypeDesc,
                    Value = item.PrintTypeId.ToString()
                }),
                ShipmentTermDropDown = _db.ShipmentTerms //
                .OrderBy(field =>
                    field.ShipmentTermName
                ).Select(item => new SelectListItem
                {
                    Text = item.ShipmentTermName,
                    Value = item.ShipmentTermId.ToString()
                }),
                CylinderCodeDropDown = _db.Cylinders //
                .OrderBy(field =>
                    field.CylinderCode
                ).Select(item => new SelectListItem
                {
                    Text = item.CylinderCode,
                    Value = item.CylinderId.ToString()
                })
            };

            List<CustomerJM> customerJMList = _db.Customers
            .Where(fields =>
                fields.IsActive == true
            ).Select(fields => new CustomerJM
            {
                Id = fields.CustomerId,
                CustomerTypeDesc =
                    (
                        _db.CustomerTypes.FirstOrDefault(row =>
                            row.CustomerTypeId == fields.CustomerTypeId
                        )
                        ?? new CustomerType { CustomerTypeDesc = "" }
                    ).CustomerTypeDesc,
                CurrencyCode =
                    (
                        _db.Currencies.FirstOrDefault(row =>
                            row.CurrencyId == fields.CurrencyId
                        )
                        ?? new Currency { CurrencyCode = "" }
                    ).CurrencyCode,
                RateInLKR =
                    (
                        _db.ExchangeRates.Where(recordE =>
                            recordE.CurrencyId == fields.CurrencyId
                            &&
                            recordE.IsActive
                            &&
                            recordE.EffectiveFrom <= polyCosting.DateCreated
                        ).OrderByDescending(fields =>
                            fields.EffectiveFrom
                        ).FirstOrDefault()
                        ?? new ExchangeRate { RateInLKR = 0 }
                    ).RateInLKR,
                CustomerAddress = fields.CustomerAddress ?? ""
            }
            ).ToList();
            ViewBag.customersJsonStr = JsonSerializer.Serialize(customerJMList);

            List<PolyCombinationJM> polyCombinationJMList = _db.PolyCombinations
            .Where(fieldsA =>
                fieldsA.IsActive
            ).OrderBy(fieldsA =>
                fieldsA.PolyCombCode
            ).Select(fieldsB => new PolyCombinationJM
            {
                Id = fieldsB.PolyCombId,
                ColorId = fieldsB.ColorId,
                PolyMaterialId = fieldsB.PolyMaterialId,
                PolyCombCode = fieldsB.PolyCombCode,
                PolyCombDesc = fieldsB.PolyCombDesc,
                PolyCombItemCatBridges = _db.PolyCombItemCatBridges
                    .Where(fieldsC =>
                        fieldsC.PolyCombId == fieldsB.PolyCombId
                    ).Select(fields2 => new PolyCombItemCatBridgeJM
                    {
                        Id = fields2.ItemCatId,
                        Percentage = fields2.Percentage,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(fields3 =>
                                    fields3.IsActive
                                    && fields3.ItemCatId == fields2.ItemCatId
                                    && fields3.DateUpdated <= polyCosting.DateCreated
                                    ).OrderByDescending(fields5 =>
                                    fields5.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitPrice = 0d })
                                ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(fields4 =>
                                    fields4.ItemCatId == fields2.ItemCatId
                                    && fields4.DateUpdated <= polyCosting.DateCreated
                                    ).OrderByDescending(fields6 =>
                                    fields6.DateUpdated
                                    ).FirstOrDefault()
                                    ?? (new ItemCategoryPrice { UnitDuty = 0d })
                                ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.polyCombinationJsonStr = JsonSerializer.Serialize(polyCombinationJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC").ItemCatId
                    && fields.DateUpdated <= polyCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.BlockCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "BC").ItemCatId
                    && fields.DateUpdated <= polyCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.OtherCosts = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OC").ItemCatId
                    && fields.DateUpdated <= polyCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            List<BagTypeJM> bagTypeJMList = _db.BagTypes
                .Where(fields =>
                    fields.Isactive == true
                ).Select(fields => new BagTypeJM
                {
                    Id = fields.BagTypeId,
                    BagTypeDesc = fields.BagTypeDesc,
                    ProductFinishingTypeDesc =
                    (
                        _db.ProductFinishingTypes.FirstOrDefault(fields2 =>
                            fields2.ProductFinTyId == fields.ProductFinishingTypeId
                        )
                        ?? new ProductFinishingType { ProductFinishingTypeDesc = "" }
                    ).ProductFinishingTypeDesc,
                    PrintByWidthOrLength = fields.PrintByWidthOrLength,
                    FormulaToCalcSArea = fields.FormulaToCalcSArea,
                    SheetSizeFormula = fields.SheetSizeFormula
                }).ToList();
            ViewBag.bagTypeJsonStr = JsonSerializer.Serialize(bagTypeJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<CylinderJM> cylinderJMList = _db.Cylinders
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new CylinderJM
                {
                    Id = fields.CylinderId,
                    CylinderCode = fields.CylinderCode,
                    CylinderSizeCm = fields.CylinderSizeCm,
                    NoOfColors = fields.NoOfColors
                }).ToList();
            ViewBag.cylinderJsonStr = JsonSerializer.Serialize(cylinderJMList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(ic => 
                    ic.IsPolyComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            polyCosting.Customer = _db.Customers
                .Find(polyCosting.CustomerId);
            if(polyCosting.Customer != null)
            {
                polyCosting.Customer.SCoordinator = _db.SRepsOrCoordinators
                .Find(polyCosting.Customer.SCoordinatorId);
                polyCosting.Customer.SRep = _db.SRepsOrCoordinators
                .Find(polyCosting.Customer.SRepId);
            }

            // To get item name these must be kept filled
            polyCosting.MeasurementUom = _db.Uoms.Find(polyCosting.MeasurementUomId);
            polyCosting.PolyCombination = _db.PolyCombinations.Find(polyCosting.PolyCombId);
            if(polyCosting.PolyCombination != null)
            {
                polyCosting.PolyCombination.PolyMaterial = _db.PolyMaterials.Find(polyCosting.PolyCombination.PolyMaterialId);
                polyCosting.PolyCombination.Color = _db.Colors.Find(polyCosting.PolyCombination.ColorId);
            }
            polyCosting.BagType = _db.BagTypes.Find(polyCosting.BagTypeId);

            return View(polyCostingVM);
        }

    }
}
