using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace Costing_Formula___KSPA.Controllers
{
    public class CorruCostingController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CorruCostingController(ApplicationDbContext db)
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

            // We no longer fetch data here. We just return the empty View.
            return View();
        }

        [HttpPost]
        public IActionResult GetCostingsData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // 2. Base Query
            IQueryable<CorruCosting> query = _db.CorruCostings
                .Include(q => q.Customer).ThenInclude(q => q.Currency)
                .Include(q => q.Customer).ThenInclude(q => q.SRep)
                .Include(q => q.CorruCombination)
                    .ThenInclude(c => c.CorruCombItemCatBridges)
                        .ThenInclude(b => b.Placement);

            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
            {
                var currentUserName = User.Identity.Name?.ToLower();
                query = query.Where(fields =>
                    (fields.CreatedUser ?? "null").ToLower().Equals(currentUserName)
                    || fields.CreatedUser == null);
            }

            recordsTotal = query.Count();

            // 3. Searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                query = query.Where(m =>
                    m.CorruCostingId.ToString().Contains(searchValue) ||
                    (m.Customer != null && m.Customer.CustomerShortCode != null && m.Customer.CustomerShortCode.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.CustomerName != null && m.Customer.CustomerName.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.Currency != null && m.Customer.Currency.CurrencyCode != null && m.Customer.Currency.CurrencyCode.ToLower().Contains(searchValue)) ||
                    (m.Customer != null && m.Customer.SRep != null && m.Customer.SRep.SrocName != null && m.Customer.SRep.SrocName.ToLower().Contains(searchValue)) ||

                    // Search against the underlying DB fields that make up "Item Name"
                    (m.BoxType != null && m.BoxType.BoxTypeDesc != null && m.BoxType.BoxTypeDesc.ToLower().Contains(searchValue)) ||
                    (m.CorruProductFinishingType != null && m.CorruProductFinishingType.Desc != null && m.CorruProductFinishingType.Desc.ToLower().Contains(searchValue)) ||
                    (m.CorruCombination != null && m.CorruCombination.Color != null && m.CorruCombination.Color.ColorName != null && m.CorruCombination.Color.ColorName.ToLower().Contains(searchValue)) ||
                    (m.CorruCombination != null && m.CorruCombination.CorruCombCode != null && m.CorruCombination.CorruCombCode.ToLower().Contains(searchValue))
                );
            }

            int recordsFiltered = query.Count();

            // 4. Sorting 
            if (!string.IsNullOrEmpty(sortColumnIndex) && !string.IsNullOrEmpty(sortColumnDir))
            {
                bool isDesc = sortColumnDir == "desc";
                query = sortColumnIndex switch
                {
                    "0" => isDesc ? query.OrderByDescending(c => c.CorruCostingId) : query.OrderBy(c => c.CorruCostingId),
                    "1" => isDesc ? query.OrderByDescending(c => c.CorruCostingCode) : query.OrderBy(c => c.CorruCostingCode),
                    "2" => isDesc ? query.OrderByDescending(c => c.DateCreated) : query.OrderBy(c => c.DateCreated),
                    "3" => isDesc ? query.OrderByDescending(c => c.Customer.CustomerName) : query.OrderBy(c => c.Customer.CustomerName),
                    "4" => isDesc ? query.OrderByDescending(c => c.Customer.CustomerShortCode) : query.OrderBy(c => c.Customer.CustomerShortCode),

                    // FIXED: Sort by physical dimensions since ItemName is NotMapped
                    "5" => isDesc ? query.OrderByDescending(c => c.Length).ThenByDescending(c => c.Bredth) : query.OrderBy(c => c.Length).ThenBy(c => c.Bredth),

                    "6" => isDesc ? query.OrderByDescending(c => c.OrderQty) : query.OrderBy(c => c.OrderQty),
                    "8" => isDesc ? query.OrderByDescending(c => c.Customer.Currency.CurrencyCode) : query.OrderBy(c => c.Customer.Currency.CurrencyCode),
                    "9" => isDesc ? query.OrderByDescending(c => c.PerUnitAdjustableUnitPrice) : query.OrderBy(c => c.PerUnitAdjustableUnitPrice),
                    "11" => isDesc ? query.OrderByDescending(c => c.PerUnitAdjustableUnitPrice * c.OrderQty) : query.OrderBy(c => c.PerUnitAdjustableUnitPrice * c.OrderQty),
                    "13" => isDesc ? query.OrderByDescending(c => c.Customer.SRep.SrocName) : query.OrderBy(c => c.Customer.SRep.SrocName),
                    _ => isDesc ? query.OrderByDescending(c => c.CorruCostingId) : query.OrderBy(c => c.CorruCostingId)
                };
            }

            // 5. Pagination
            var pagedData = query.Skip(skip).Take(pageSize).ToList();

            // Dictionary to hold our exactly-calculated weights
            Dictionary<int, double> calculatedWeights = new Dictionary<int, double>();

            // 6. N+1 Optimization for Exchange Rates & Exact JS Weight Algorithm
            foreach (var c in pagedData)
            {
                // --- EXCHANGE RATE ---
                if (c.Customer?.CurrencyId != null)
                {
                    c.SelectedExchangeRate = _db.ExchangeRates
                        .Where(recordE => recordE.CurrencyId == c.Customer.CurrencyId
                                       && recordE.EffectiveFrom <= c.DateCreated)
                        .OrderByDescending(fields => fields.EffectiveFrom)
                        .Select(e => e.RateInLKR)
                        .FirstOrDefault();
                }
                else
                {
                    c.SelectedExchangeRate = 0;
                }

                // --- EXACT WEIGHT CALCULATION ALGORITHM FROM JAVASCRIPT ---
                // Fetch Route to get waste percentages 
                var route = _db.Routes?.FirstOrDefault(r => r.RouteId == c.RouteId);
                double fixedExcess = route != null ? route.FixedExcess : 0;
                double handlingWastePercent = route != null ? route.HandlingWastePercent : 0;

                double totalWeight = 0;

                // Match JS Variables exactly
                double ordQty = c.OrderQty;
                double noOfBoards = c.NoOfBoards;
                double adjSheetWidth = c.AdjSheetWidth;
                double adjSheetLength = c.AdjSheetLength;
                double ups = c.Ups;
                double reelSize = Math.Round((double)c.BestReelSize, 4, MidpointRounding.AwayFromZero);
                double preCWastePercent = c.PreCostingWastePercentage;

                // JS: var sheetWasteWidth = Math.round(((reelSize - adjSheetWidth * ups) / ups) * 10000) / 10000;
                double sheetWasteWidth = 0;
                if (ups > 0)
                {
                    sheetWasteWidth = Math.Round(((reelSize - (adjSheetWidth * ups)) / ups), 4, MidpointRounding.AwayFromZero);
                }

                if (c.CorruCombination?.CorruCombItemCatBridges != null)
                {
                    foreach (var bridge in c.CorruCombination.CorruCombItemCatBridges)
                    {
                        double gsm = bridge.Gsm;
                        double factor = Math.Round((double)(bridge.Placement?.PlacementFactor ?? 1.0), 4, MidpointRounding.AwayFromZero);

                        // JS: var stdConsumption = Math.round((adjSheetWidth * adjSheetLength * gsm * factor / 10000000) * 10000) / 10000;
                        double stdConsumption = Math.Round((adjSheetWidth * adjSheetLength * gsm * factor / 10000000.0), 4, MidpointRounding.AwayFromZero);

                        // JS: var stdWaste = Math.round((sheetWasteWidth * adjSheetLength * gsm * factor / 10000000) * 10000) / 10000;
                        double stdWaste = Math.Round((sheetWasteWidth * adjSheetLength * gsm * factor / 10000000.0), 4, MidpointRounding.AwayFromZero);

                        // JS: var preCostingWaste = Math.round(((stdConsumption + stdWaste) * Number(preCWastePercentInputF.val()) / 100) * 10000) / 10000;
                        double preCostingWaste = Math.Round(((stdConsumption + stdWaste) * preCWastePercent / 100.0), 4, MidpointRounding.AwayFromZero);

                        double conversionWaste = 0;
                        if (ordQty > 0 && noOfBoards > 0)
                        {
                            // JS: Math.round(Number(fixedExcessInputF.val()) + ordQty * Number(handlingWastePercentInputF.val()) / 10000)
                            // Notice: It rounds to the nearest WHOLE NUMBER because there is no division by 10000 after Math.round!
                            double extraQty = Math.Round(fixedExcess + (ordQty * handlingWastePercent / 10000.0), MidpointRounding.AwayFromZero);

                            // JS: ((... * extraQty / ordQty) / noOfBoards)
                            double rawConvWaste = (((stdConsumption + stdWaste + preCostingWaste) * extraQty / ordQty) / noOfBoards);
                            conversionWaste = Math.Round(rawConvWaste, 4, MidpointRounding.AwayFromZero);
                        }

                        // JS: var costingWeight = Math.round((stdConsumption + stdWaste + preCostingWaste + conversionWaste) * 10000) / 10000;
                        double costingWeight = Math.Round((stdConsumption + stdWaste + preCostingWaste + conversionWaste), 4, MidpointRounding.AwayFromZero);

                        // JS: var unitWeight = Math.round(costingWeight * noOfBoards * 10000) / 10000;
                        double unitWeight = Math.Round(costingWeight * noOfBoards, 4, MidpointRounding.AwayFromZero);

                        // JS: var totalWeight = Math.round(unitWeight * ordQty * 10000) / 10000;
                        double catTotalWeight = Math.Round(unitWeight * ordQty, 4, MidpointRounding.AwayFromZero);

                        totalWeight += catTotalWeight;
                    }
                }

                calculatedWeights[c.CorruCostingId] = totalWeight;
            }

            // 7. Format Data
            var data = pagedData.Select(c => new
            {
                id = c.CorruCostingId,
                quotationNo = c.CorruCostingCode,
                docDate = c.DateCreated.ToString("yyyy/MM/dd hh:mm:ss"),
                customerName = c.Customer?.CustomerName ?? "",
                customerCode = c.Customer?.CustomerShortCode ?? "",
                itemName = c.ItemName ?? "",
                orderQty = c.OrderQty.ToString("n0"),
                weight = calculatedWeights.ContainsKey(c.CorruCostingId) ? calculatedWeights[c.CorruCostingId].ToString("n2") : "0.00",
                currencyCode = c.Customer?.Currency?.CurrencyCode ?? "",
                rateNative = c.PerUnitAdjustableUnitPrice.ToString("n2"),
                rateLkr = (c.PerUnitAdjustableUnitPrice * (c.SelectedExchangeRate ?? 0)).ToString("n2"),
                salesPriceNative = (c.PerUnitAdjustableUnitPrice * c.OrderQty).ToString("n2"),
                salesPriceLkr = (c.PerUnitAdjustableUnitPrice * c.OrderQty * (c.SelectedExchangeRate ?? 0)).ToString("n2"),
                salesRep = c.Customer?.SRep?.SrocName ?? ""
            });

            // 8. Return JSON
            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsFiltered,
                recordsTotal = recordsTotal,
                data = data
            });
        }

        // if id > 0 Create by cloning
        public IActionResult Create(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            CorruCosting? corruCosting;

            if (id == null || id == 0)
            {
                corruCosting = new CorruCosting()
                {
                    AreExternalMeasurements = 1, // Setting default val
                    DateCreated = DateTime.Now
                };
            }
            else
            {
                corruCosting = _db.CorruCostings.Find(id);
                if (corruCosting == null)
                {
                    return NotFound();
                }
                corruCosting.CorruCostingId = 0;
            }

            int mesurementIdOfOrders = _db.Measurements.First(m =>
                m.MeasurementDesc == "ORDERS"
            ).MeasurementId;

            int mesurementIdOfLength = _db.Measurements.First(m =>
                m.MeasurementDesc == "LENGTH"
            ).MeasurementId;

            // Getting View Model
            CorruCostingVM model = new CorruCostingVM()
            {
                CorruCosting = corruCosting,
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
                    && (field.ColorName.ToLower().Equals("brown")
                        || field.ColorName.ToLower().Equals("white"))
                ).OrderBy(field =>
                    field.ColorName
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PlysDropDown = _db.CorruCombinations
                .Where(cc =>
                    cc.IsActive
                ).OrderBy(cc =>
                    cc.NoOfPlys
                ).Select(cc => new SelectListItem
                {
                    Text = cc.NoOfPlys.ToString(),
                    Value = cc.NoOfPlys.ToString()
                }).Distinct(),
                BoxTypeDropDown = _db.BoxTypes
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.BoxTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.BoxTypeDesc,
                    Value = item.BoxTypeId.ToString()
                }),
                CorruCombinationDropDown = _db.CorruCombinations
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CorruCombCode
                ).Select(item => new SelectListItem
                {
                    Text = item.CorruCombCode,
                    Value = item.CorruCombId.ToString()
                }),
                OrderUomDropDown = _db.Uoms
                .Where(uom =>
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfOrders
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
                LaminateTypeDropDown = _db.LaminateTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.LaminateTypeId.ToString()
                }),
                DieCutTypeDropDown = _db.DieCutTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.DieCutTypeId.ToString()
                }),
                FinishingTypeDropDown = _db.CorruProductFinishingType
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.CorruProductFinishingTypeId.ToString()
                }),
                RouteDropDown = _db.Routes
                .OrderBy(r =>
                    r.RouteName
                ).Select(item => new SelectListItem
                {
                    Text = item.RouteName,
                    Value = item.RouteId.ToString()
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
                MeasurementUomDropDown = _db.Uoms.Where(uom =>
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfLength
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
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

            List<BoxTypeJM> boxTypeJMList = _db.BoxTypes
                .Where(bt =>
                    bt.IsActive
                ).OrderBy(bt =>
                    bt.BoxTypeDesc
                ).Select(fields => new BoxTypeJM
                {
                    Id = fields.BoxTypeId,
                    BoxTypeDesc = fields.BoxTypeDesc,
                    NoOfPlys = fields.NoOfPlys,
                    ESheetLengthFormula = fields.ESheetLengthFormula,
                    ESheetLengthFormulaIfSLExceeds = fields.ESheetLengthFormulaIfSLExceeds,
                    ESheetWidthFormula = fields.ESheetWidthFormula,
                    ISheetLengthFormula = fields.ISheetLengthFormula,
                    ISheetLengthFormulaIfSLExceeds = fields.ISheetLengthFormulaIfSLExceeds,
                    ISheetWidthFormula = fields.ISheetWidthFormula,
                    Laminate = fields.Laminate,
                    DieCut = fields.DieCut,
                    DesignBox = fields.DesignBox,
                    Board = fields.Board,
                }).ToList();
            ViewBag.boxTypeJsonStr = JsonSerializer.Serialize(boxTypeJMList);

            List<CorruCombinationJM> corruCombinationJMList = _db.CorruCombinations
            .Where(cc =>
                cc.IsActive
            ).OrderBy(cc =>
                cc.CorruCombCode
            ).Select(cc => new CorruCombinationJM
            {
                Id = cc.CorruCombId,
                ColorId = cc.ColorId,
                NoOfPlys = cc.NoOfPlys,
                CorruCombCode = cc.CorruCombCode,
                CorruCombItemCatBridges = _db.CorruCombItemCatBridges
                    .Where(ccic_b =>
                        ccic_b.CorruCombId == cc.CorruCombId
                    ).Select(ccic_b => new CorruCombItemCatBridgeJM
                    {
                        Id = ccic_b.ItemCatId,
                        PlacementId = ccic_b.PlacementId,
                        Gsm = ccic_b.Gsm,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(icp =>
                                icp.IsActive
                                && icp.ItemCatId == ccic_b.ItemCatId
                                ).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitPrice = 0d })
                            ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(ccic_b2 =>
                                ccic_b2.IsActive
                                && ccic_b2.ItemCatId == ccic_b.ItemCatId).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitDuty = 0d })
                            ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.corruCombinationJsonStr = JsonSerializer.Serialize(corruCombinationJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase,
                    PrintPercentage = fields.PrintPercentage
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<RouteJM> routeJMList = _db.Routes
                .Select(r => new RouteJM
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    FixedExcess = r.FixedExcess,
                    HandlingWastePercent = r.HandlingWastePercent
                }).ToList();
            ViewBag.routeJsonStr = JsonSerializer.Serialize(routeJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC-C").ItemCatId
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

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(field =>
                    field.IsActive
                    && field.IsCorruComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            List<PlacementJM> placementJMList = _db.Placements
                .Where(field =>
                    field.IsActive
                ).Select(fields => new PlacementJM
                {
                    Id = fields.PlacementId,
                    PlacementCode = fields.PlacementCode,
                    Factor = fields.PlacementFactor
                }).ToList();
            ViewBag.placementJsonStr = JsonSerializer.Serialize(placementJMList);

            List<ReelSizeJM> reelSizeJMList = _db.ReelSizes
                .Where(field =>
                    field.IsActive
                ).Select(fields => new ReelSizeJM
                {
                    Id = fields.ReelSizeId,
                    Size = fields.Size
                }).OrderBy(rs =>
                    rs.Size
                ).ToList();
            ViewBag.reelSizeJsonStr = JsonSerializer.Serialize(reelSizeJMList);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CorruCostingVM model)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (model == null || model.CorruCosting == null)
                return NotFound();

            model.CorruCosting.DateCreated = DateTime.Now;
            model.CorruCosting.CreatedUser = User?.Identity?.Name;

            if (ModelState.IsValid)
            {
                _db.CorruCostings.Add(model.CorruCosting);
                ViewBag.SuccessMessage = "Quotation created successfully!";
                _db.SaveChanges();

                return RedirectToAction("View", new { id = model.CorruCosting.CorruCostingId });
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
            }

            // Filling virtual properties and setting ViewBags
            int mesurementIdOfOrders = _db.Measurements.First(m =>
                m.MeasurementDesc == "ORDERS"
            ).MeasurementId;

            int mesurementIdOfLength = _db.Measurements.First(m =>
                m.MeasurementDesc == "LENGTH"
            ).MeasurementId;

            model.CompanyDropDown = _db.Companies
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CompanyName
                ).Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                });
            model.CustomerDropDown = _db.Customers
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CustomerName
                ).Select(item => new SelectListItem
                {
                    Text = item.CustomerName,
                    Value = item.CustomerId.ToString()
                });
            model.ColorDropDown = _db.Colors
                .Where(field =>
                    field.IsActive
                    && (field.ColorName.ToLower().Equals("brown")
                        || field.ColorName.ToLower().Equals("white"))
                ).OrderBy(field =>
                    field.ColorName
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                });
            model.PlysDropDown = _db.CorruCombinations
                .Where(cc =>
                    cc.IsActive
                ).OrderBy(cc =>
                    cc.NoOfPlys
                ).Select(cc => new SelectListItem
                {
                    Text = cc.NoOfPlys.ToString(),
                    Value = cc.NoOfPlys.ToString()
                }).Distinct();
            model.BoxTypeDropDown = _db.BoxTypes
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.BoxTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.BoxTypeDesc,
                    Value = item.BoxTypeId.ToString()
                });
            model.CorruCombinationDropDown = _db.CorruCombinations
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.CorruCombCode
                ).Select(item => new SelectListItem
                {
                    Text = item.CorruCombCode,
                    Value = item.CorruCombId.ToString()
                });
            model.OrderUomDropDown = _db.Uoms
                .Where(uom => //
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfOrders
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                });
            model.PrintTypeDropDown = _db.PrintTypes
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.PrintTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.PrintTypeDesc,
                    Value = item.PrintTypeId.ToString()
                });
            model.LaminateTypeDropDown = _db.LaminateTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.LaminateTypeId.ToString()
                });
            model.DieCutTypeDropDown = _db.DieCutTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.DieCutTypeId.ToString()
                });
            model.FinishingTypeDropDown = _db.CorruProductFinishingType
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.CorruProductFinishingTypeId.ToString()
                });
            model.RouteDropDown = _db.Routes
                .OrderBy(r =>
                    r.RouteName
                ).Select(item => new SelectListItem
                {
                    Text = item.RouteName,
                    Value = item.RouteId.ToString()
                });
            model.ShipmentTermDropDown = _db.ShipmentTerms
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.ShipmentTermName
                ).Select(item => new SelectListItem
                {
                    Text = item.ShipmentTermName,
                    Value = item.ShipmentTermId.ToString()
                });
            model.MeasurementUomDropDown = _db.Uoms.Where(uom =>
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfLength
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
                });

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

            List<BoxTypeJM> boxTypeJMList = _db.BoxTypes
                .Where(bt =>
                    bt.IsActive
                ).OrderBy(bt =>
                    bt.BoxTypeDesc
                ).Select(fields => new BoxTypeJM
                {
                    Id = fields.BoxTypeId,
                    BoxTypeDesc = fields.BoxTypeDesc,
                    NoOfPlys = fields.NoOfPlys,
                    ESheetLengthFormula = fields.ESheetLengthFormula,
                    ESheetLengthFormulaIfSLExceeds = fields.ESheetLengthFormulaIfSLExceeds,
                    ESheetWidthFormula = fields.ESheetWidthFormula,
                    ISheetLengthFormula = fields.ISheetLengthFormula,
                    ISheetLengthFormulaIfSLExceeds = fields.ISheetLengthFormulaIfSLExceeds,
                    ISheetWidthFormula = fields.ISheetWidthFormula,
                    Laminate = fields.Laminate,
                    DieCut = fields.DieCut,
                    DesignBox = fields.DesignBox,
                    Board = fields.Board,
                }).ToList();
            ViewBag.boxTypeJsonStr = JsonSerializer.Serialize(boxTypeJMList);

            List<CorruCombinationJM> corruCombinationJMList = _db.CorruCombinations
            .Where(cc =>
                cc.IsActive
            ).OrderBy(cc =>
                cc.CorruCombCode
            ).Select(cc => new CorruCombinationJM
            {
                Id = cc.CorruCombId,
                ColorId = cc.ColorId,
                NoOfPlys = cc.NoOfPlys,
                CorruCombCode = cc.CorruCombCode,
                CorruCombItemCatBridges = _db.CorruCombItemCatBridges
                    .Where(ccic_b =>
                        ccic_b.CorruCombId == cc.CorruCombId
                    ).Select(ccic_b => new CorruCombItemCatBridgeJM
                    {
                        Id = ccic_b.ItemCatId,
                        PlacementId = ccic_b.PlacementId,
                        Gsm = ccic_b.Gsm,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(icp =>
                                icp.IsActive
                                && icp.ItemCatId == ccic_b.ItemCatId
                                ).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitPrice = 0d })
                            ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(ccic_b2 =>
                                ccic_b2.IsActive
                                && ccic_b2.ItemCatId == ccic_b.ItemCatId).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitDuty = 0d })
                            ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.corruCombinationJsonStr = JsonSerializer.Serialize(corruCombinationJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase,
                    PrintPercentage = fields.PrintPercentage
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<RouteJM> routeJMList = _db.Routes
                .Select(r => new RouteJM
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    FixedExcess = r.FixedExcess,
                    HandlingWastePercent = r.HandlingWastePercent
                }).ToList();
            ViewBag.routeJsonStr = JsonSerializer.Serialize(routeJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC-C").ItemCatId
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

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(field =>
                    field.IsActive
                    && field.IsCorruComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            List<PlacementJM> placementJMList = _db.Placements
                .Where(field =>
                    field.IsActive
                ).Select(fields => new PlacementJM
                {
                    Id = fields.PlacementId,
                    PlacementCode = fields.PlacementCode,
                    Factor = fields.PlacementFactor
                }).ToList();
            ViewBag.placementJsonStr = JsonSerializer.Serialize(placementJMList);

            List<ReelSizeJM> reelSizeJMList = _db.ReelSizes
                .Where(field =>
                    field.IsActive
                ).Select(fields => new ReelSizeJM
                {
                    Id = fields.ReelSizeId,
                    Size = fields.Size
                }).OrderBy(rs =>
                    rs.Size
                ).ToList();
            ViewBag.reelSizeJsonStr = JsonSerializer.Serialize(reelSizeJMList);

            return View(model);
        }

        public IActionResult View(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var corruCosting = _db.CorruCostings
                .Include(cc => cc.MeasurementUom)
                .Include(cc => cc.Customer).ThenInclude(cus => cus.SCoordinator)
                .Include(cc => cc.Customer).ThenInclude(cus => cus.SRep)
                .Include(cc => cc.CorruCombination).ThenInclude(cComb => cComb.Color)
                .Include(cc => cc.BoxType)
                .Include(cc => cc.CorruProductFinishingType)
                .FirstOrDefault(cc =>
                    cc.CorruCostingId == id
                );
            if (corruCosting == null)
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
            CorruCostingVM model = new CorruCostingVM()
            {
                CorruCosting = corruCosting,
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
                    && (field.ColorName.ToLower().Equals("brown")
                        || field.ColorName.ToLower().Equals("white"))
                ).OrderBy(field =>
                    field.ColorName
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PlysDropDown = _db.CorruCombinations
                //.Where(cc =>
                //    cc.IsActive
                //)
                .OrderBy(cc =>
                    cc.NoOfPlys
                ).Select(cc => new SelectListItem
                {
                    Text = cc.NoOfPlys.ToString(),
                    Value = cc.NoOfPlys.ToString()
                }).Distinct(),
                BoxTypeDropDown = _db.BoxTypes
                .Where(field =>
                    field.IsActive
                ).OrderBy(field =>
                    field.BoxTypeDesc
                ).Select(item => new SelectListItem
                {
                    Text = item.BoxTypeDesc,
                    Value = item.BoxTypeId.ToString()
                }),
                CorruCombinationDropDown = _db.CorruCombinations
                // Where clause is not required in the View Only screen.
                //.Where(field =>
                //    field.IsActive
                //)
                .OrderBy(field =>
                    field.CorruCombCode
                ).Select(item => new SelectListItem
                {
                    Text = item.CorruCombCode,
                    Value = item.CorruCombId.ToString()
                }),
                OrderUomDropDown = _db.Uoms
                .Where(uom =>
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfOrders
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
                LaminateTypeDropDown = _db.LaminateTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.LaminateTypeId.ToString()
                }),
                DieCutTypeDropDown = _db.DieCutTypes
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.DieCutTypeId.ToString()
                }),
                FinishingTypeDropDown = _db.CorruProductFinishingType
                .OrderBy(field =>
                    field.Desc
                ).Select(item => new SelectListItem
                {
                    Text = item.Desc,
                    Value = item.CorruProductFinishingTypeId.ToString()
                }),
                RouteDropDown = _db.Routes
                .OrderBy(r =>
                    r.RouteName
                ).Select(item => new SelectListItem
                {
                    Text = item.RouteName,
                    Value = item.RouteId.ToString()
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
                MeasurementUomDropDown = _db.Uoms.Where(uom =>
                    uom.IsActive == true
                    &&
                    uom.MeasurementId == mesurementIdOfLength
                ).Select(item => new SelectListItem
                {
                    Text = item.UomCode,
                    Value = item.UomId.ToString()
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
                            recordE.EffectiveFrom <= corruCosting.DateCreated
                        ).OrderByDescending(fields =>
                            fields.EffectiveFrom
                        ).FirstOrDefault()
                        ?? new ExchangeRate { RateInLKR = 0 }
                    ).RateInLKR,
                CustomerAddress = fields.CustomerAddress ?? ""
            }
            ).ToList();
            ViewBag.customersJsonStr = JsonSerializer.Serialize(customerJMList);

            List<BoxTypeJM> boxTypeJMList = _db.BoxTypes
                .Where(bt =>
                    bt.IsActive
                ).OrderBy(bt =>
                    bt.BoxTypeDesc
                ).Select(fields => new BoxTypeJM
                {
                    Id = fields.BoxTypeId,
                    BoxTypeDesc = fields.BoxTypeDesc,
                    NoOfPlys = fields.NoOfPlys,
                    ESheetLengthFormula = fields.ESheetLengthFormula,
                    ESheetLengthFormulaIfSLExceeds = fields.ESheetLengthFormulaIfSLExceeds,
                    ESheetWidthFormula = fields.ESheetWidthFormula,
                    ISheetLengthFormula = fields.ISheetLengthFormula,
                    ISheetLengthFormulaIfSLExceeds = fields.ISheetLengthFormulaIfSLExceeds,
                    ISheetWidthFormula = fields.ISheetWidthFormula,
                    Laminate = fields.Laminate,
                    DieCut = fields.DieCut,
                    DesignBox = fields.DesignBox,
                    Board = fields.Board,
                }).ToList();
            ViewBag.boxTypeJsonStr = JsonSerializer.Serialize(boxTypeJMList);

            List<CorruCombinationJM> corruCombinationJMList = _db.CorruCombinations
            //.Where(cc =>
            //    cc.IsActive
            //)
            .OrderBy(cc =>
                cc.CorruCombCode
            ).Select(cc => new CorruCombinationJM
            {
                Id = cc.CorruCombId,
                ColorId = cc.ColorId,
                NoOfPlys = cc.NoOfPlys,
                CorruCombCode = cc.CorruCombCode,
                CorruCombItemCatBridges = _db.CorruCombItemCatBridges
                    .Where(ccic_b =>
                        ccic_b.CorruCombId == cc.CorruCombId
                    ).Select(ccic_b => new CorruCombItemCatBridgeJM
                    {
                        Id = ccic_b.ItemCatId,
                        PlacementId = ccic_b.PlacementId,
                        Gsm = ccic_b.Gsm,
                        UnitPrice = (_db.ItemCategoryPrices
                            .Where(icp =>
                                icp.IsActive
                                && icp.ItemCatId == ccic_b.ItemCatId
                                && icp.DateUpdated <= corruCosting.DateCreated
                                ).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitPrice = 0d })
                            ).UnitPrice,
                        UnitDuty = (_db.ItemCategoryPrices
                            .Where(ccic_b2 =>
                                ccic_b2.IsActive
                                && ccic_b2.ItemCatId == ccic_b.ItemCatId
                                && ccic_b2.DateUpdated <= corruCosting.DateCreated
                                ).OrderByDescending(ccic_b =>
                                ccic_b.DateUpdated
                                ).FirstOrDefault()
                                ?? (new ItemCategoryPrice { UnitDuty = 0d })
                            ).UnitDuty
                    }).ToList()
            }).ToList();
            ViewBag.corruCombinationJsonStr = JsonSerializer.Serialize(corruCombinationJMList);

            List<UomJM> uomJMsList = _db.Uoms
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new UomJM
                {
                    Id = fields.UomId,
                    ConversionMultiplierToSystemUom = fields.ConversionMultiplierToSystemUom
                }).ToList();
            ViewBag.uomJsonStr = JsonSerializer.Serialize(uomJMsList);

            List<PrintTypeJM> printTypeJMList = _db.PrintTypes
                .Where(fields =>
                    fields.IsActive == true
                ).Select(fields => new PrintTypeJM
                {
                    Id = fields.PrintTypeId,
                    WithoutCylinderBase = fields.WithoutCylinderBase,
                    PrintPercentage = fields.PrintPercentage
                }).ToList();
            ViewBag.printTypeJsonStr = JsonSerializer.Serialize(printTypeJMList);

            List<RouteJM> routeJMList = _db.Routes
                .Select(r => new RouteJM
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    FixedExcess = r.FixedExcess,
                    HandlingWastePercent = r.HandlingWastePercent
                }).ToList();
            ViewBag.routeJsonStr = JsonSerializer.Serialize(routeJMList);

            ViewBag.OverHeadCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OHC-C").ItemCatId
                    && fields.DateUpdated <= corruCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.BlockCost = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "BC").ItemCatId
                    && fields.DateUpdated <= corruCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            ViewBag.OtherCosts = (_db.ItemCategoryPrices
                .OrderByDescending(fields => fields.DateUpdated)
                .FirstOrDefault(fields =>
                    fields.IsActive
                    && fields.ItemCatId == _db.ItemCategories.First(fields => fields.ItemCategoryCode == "OC").ItemCatId
                    && fields.DateUpdated <= corruCosting.DateCreated
                )
                ?? (new ItemCategoryPrice { UnitPrice = 0 })
            ).UnitPrice;

            List<ItemCategoryJM> itemCategoryJMList = _db.ItemCategories
                .Where(field =>
                    field.IsActive
                    && field.IsCorruComb
                ).Select(fields => new ItemCategoryJM
                {
                    Id = fields.ItemCatId,
                    ItemCategoryDesc = fields.ItemCategoryDesc
                }).ToList();
            ViewBag.itemCategoryJsonStr = JsonSerializer.Serialize(itemCategoryJMList);

            List<PlacementJM> placementJMList = _db.Placements
                .Where(field =>
                    field.IsActive
                ).Select(fields => new PlacementJM
                {
                    Id = fields.PlacementId,
                    PlacementCode = fields.PlacementCode,
                    Factor = fields.PlacementFactor
                }).ToList();
            ViewBag.placementJsonStr = JsonSerializer.Serialize(placementJMList);

            List<ReelSizeJM> reelSizeJMList = _db.ReelSizes
                .Where(field =>
                    field.IsActive
                ).Select(fields => new ReelSizeJM
                {
                    Id = fields.ReelSizeId,
                    Size = fields.Size
                }).OrderBy(rs =>
                    rs.Size
                ).ToList();
            ViewBag.reelSizeJsonStr = JsonSerializer.Serialize(reelSizeJMList);

            return View(model);
        }

    }
}
