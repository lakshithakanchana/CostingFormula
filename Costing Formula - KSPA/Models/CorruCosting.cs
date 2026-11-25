using NuGet.ContentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Costing_Formula___KSPA.Models
{
    public class CorruCosting
    {
        [Key]
        public int CorruCostingId { get; set; } //PK

        [Required]
        public DateTime DateCreated { get; set; }

        public string? CreatedUser { get; set; }

        // virtual notmapped variable 1
        [NotMapped]
        public virtual string? CorruCostingCode  //Ax Key(Quotation No: PQTN/01-22/0000001=> PQTN/mmYY/0000000)
        {
            get
            {
                return $"CQTN/{this.DateCreated:dd-MM/}{CorruCostingId:D7}";
            }
            set
            {
                CorruCostingCode = value;
            }
        }

        /*
         * Description: L(length)*B(bredth)*H(height)*<->(uom)<->(INT/EXT)<->(paper_color)<->(comb_code)<->(Printed/Unprinted)<->(no_of_colors_if_printed)<->(Laminated)<->(Die)<->(box_type)<->(Stitched/Glued)<->(special_ref)
         */
        [NotMapped]
        public virtual string? ItemName
        {
            get
            {
                string itemName = "";
                itemName += ("L" + Length);
                if (Bredth > 0)
                {
                    itemName += ("*B" + Bredth);
                }
                if (Height > 0)
                {
                    itemName += ("*H" + Height);
                }
                if (MeasurementUom != null)
                {
                    itemName += (" " + MeasurementUom.UomCode);
                }
                if (AreExternalMeasurements == 1)
                {
                    itemName += (" " + "EXT");
                }
                else
                {
                    itemName += (" " + "INT");
                }
                if (CorruCombination != null)
                {
                    if (CorruCombination.Color != null)
                    {
                        itemName += (" " + CorruCombination.Color.ColorName);
                    }
                    itemName += (" " + CorruCombination.CorruCombCode);
                }
                if (Printed)
                {
                    itemName += (" Printed");
                }
                if (Laminated)
                {
                    itemName += (" Laminated");
                }
                if (DieCut)
                {
                    itemName += (" Die");
                }
                if(BoxType != null)
                {
                    itemName += (" " + BoxType.BoxTypeDesc);
                }
                if(CorruProductFinishingType != null)
                {
                    itemName += (" " + CorruProductFinishingType.Desc);
                }
                if (SpecialInstructions != null)
                {
                    itemName += (" " + SpecialInstructions);
                }

                return itemName.ToUpper();
            }
            set
            {
                CorruCostingCode = value;
            }
        }

        [Required]
        public int CompanyId { get; set; } // FK

        [Required]
        public int CustomerId { get; set; } // FK - inside this currency object also av.

        [Required]
        public int BoxTypeId { get; set; } // FK - inside this productfinishingtype, uom, diamention objectalso av

        [Required]
        public int CorruCombId { get; set; } // FK - inside this color, poly-material and polycombitemcatbridges-Ienum objects also av.

        [Required]
        public int OrderUomId { get; set; } // FK

        public int? PrintTypeId { get; set; } //FK

        public int? LaminateTypeId { get; set; } // FK

        public int? DieCutTypeId { get; set; } // FK

        public int? CorruProductFinishingTypeId { get; set; } // FK

        [Required]
        public int ShipmentTermId { get; set; } //FK

        [Required]
        public int MeasurementUomId { get; set; } //FK

        [Required]
        public int RouteId { get; set; } //FK

        public string? DeliveryAddress { get; set; }

        [Required, Range(0.000001, double.MaxValue, ErrorMessage = "Order qty must be larger than 0.")]
        public double OrderQty { get; set; }

        public string? SpecialInstructions { get; set; }

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "Length must be larger than 0.")]
        public double Length { get; set; }

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "Bredth must be larger than 0.")]
        public double Bredth { get; set; }

        [Required]
        public double Height { get; set; }

        [Required, Range(0, 1, ErrorMessage = "Accepted either external or internal.")]
        public int AreExternalMeasurements { get; set; }

        [Required]
        public bool Printed { get; set; }

        [Required]
        public bool Laminated { get; set; }

        [Required]
        public bool DieCut { get; set; }

        [Required]
        public int NoOfColors { get; set; }//?ok?

        [Required]
        [Range(0, 12, ErrorMessage = "Printing sides count is between 0-12.")]
        public int NoOfPrintingSides { get; set; } // 0 <= this <=2

        [Required]
        [Range(0, 4, ErrorMessage = "Invalid selection.")]
        public int BlockAvailability { get; set; } // 1 - New, 2 - Available, 3 - Balance Cut, 4 - Digital Printed, 0 - Not Selected

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Sheet length is 0.0000. Please check, seems you are missing some diamentions. Like length, bredth etc.")]
        public double SheetLength { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Adjusted Sheet length is 0.0000.")]
        public double AdjSheetLength { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Sheet width is 0.0000. Please check, seems you are missing some diamentions. Like length, bredth etc.")]
        public double SheetWidth { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Adjusted Sheet width is 0.0000.")]
        public double AdjSheetWidth { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Area is 0.0000. Please check, seems you are missing some diamentions. Like length, bredth etc.")]
        public double Area { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Area after adjusting is 0.0000.")]
        public double AreaAfterAdj { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Trim must be greater than 0.0000.")]
        public double Trim { get; set; }

        [Required]
        [Range(0.00001, int.MaxValue, ErrorMessage = "No of boards must be greater than 0.")]
        public int NoOfBoards { get; set; }

        [Required]
        [Range(0.00001, int.MaxValue, ErrorMessage = "Best reel size must be greater than 0.")]
        public int BestReelSize { get; set; }

        [Required]
        [Range(5, Double.MaxValue)]
        public double PreCostingWastePercentage { get; set; }

        [Required]
        [Range(0.00001, int.MaxValue, ErrorMessage = "Total sheet req. must be greater than 0.")]
        public int TotalSheetReq { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Total sheet length must be greater than 0.")]
        public double TotalSheetlength { get; set; }

        [Required]
        [Range(0.00001, int.MaxValue, ErrorMessage = "Total board req. must be greater than 0.")]
        public int TotalBoardReq { get; set; }

        [Required]
        public double BlockCostTotal { get; set; }

        [Required]
        public double LaminateCostTotal { get; set; }

        [Required]
        public double DieCostTotal { get; set; }

        [Required]
        public double OtherCostTotal { get; set; }

        [Required]
        public double PerUnitSuggestedUnitPrice { get; set; } // Native Currency

        [Required]
        public double PerUnitAdjustableUnitPrice { get; set; } // Native Currency

        [Required, Range(0.00001, int.MaxValue)]
        public int Ups { get; set; }

        // Adding virtual variables
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("BoxTypeId")]
        public virtual BoxType? BoxType { get; set; }

        [ForeignKey("CorruCombId")]
        public virtual CorruCombination? CorruCombination { get; set; }

        [ForeignKey("OrderUomId")]
        public virtual Uom? OrderUom { get; set; }

        [ForeignKey("PrintTypeId")]
        public virtual PrintType? PrintType { get; set; }

        [ForeignKey("LaminateTypeId")]
        public virtual LaminateType? LaminateType { get; set; }

        [ForeignKey("DieCutTypeId")]
        public virtual DieCutType? DieCutType { get; set; }

        [ForeignKey("CorruProductFinishingTypeId")]
        public virtual CorruProductFinishingType? CorruProductFinishingType { get; set; }

        [ForeignKey("ShipmentTermId")]
        public virtual ShipmentTerm? ShipmentTerm { get; set; }

        [ForeignKey("MeasurementUomId")]
        public virtual Uom? MeasurementUom { get; set; }
        [NotMapped]
        public virtual double? SelectedExchangeRate { get; set; }
    }
}
