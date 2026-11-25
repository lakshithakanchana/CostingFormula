using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class PolyCosting
    {
        [Key]
        public int PolyCostingId { get; set; } //PK

        [Required]
        public DateTime DateCreated { get; set; }

        public string? CreatedUser { get; set; }

        // virtual notmapped variable 1
        [NotMapped]
        public virtual string? PolyCostingCode  //Ax Key(Quotation No: PQTN/01-22/0000001=> PQTN/mmYY/0000000)
        {
            get
            {
                return $"PQTN/{this.DateCreated:dd-MM/}{PolyCostingId:D7}";
            }
            set
            {
                PolyCostingCode = value;
            }
        }

        /*
         * Description: G(gauge)*W(width)*G(guesset)*L(length)*F(flap)*B(bottemfap)<->(measurement uom)<->(Poly Combination Desc)<->(Bag Type)<->(customer ref)
         */
        [NotMapped]
        public virtual string? ItemName  //Ax Key(Quotation No: PQTN/01-22/0000001=> PQTN/mmYY/0000000)
        {
            get
            {
                string itemName = "";
                if (Guage > 0)
                {
                    itemName += ("G" + Guage);
                }
                if (Width > 0)
                {
                    itemName += ("*W" + Width);
                }
                if (Gusset > 0)
                {
                    itemName += ("*G" + Gusset);
                }
                if (Length > 0)
                {
                    itemName += ("*L" + Length);
                }
                if (FlapWidth > 0)
                {
                    itemName += ("*F" + FlapWidth);
                }
                if (BottomGusset > 0)
                {
                    itemName += ("*B" + BottomGusset);
                }
                if (MeasurementUom != null)
                {
                    itemName += (" " + MeasurementUom.UomCode);
                }
                if (PolyCombination != null)
                {
                    if (PolyCombination.PolyMaterial != null)
                    {
                        itemName += (" " + PolyCombination.PolyCombDesc);
                    }
                }
                if (BagType != null)
                {
                    itemName += (" " + BagType.BagTypeDesc);
                }
                if (Printed)
                {
                    itemName += (" Printed");
                }
                if (SpecialInstructions != null)
                {
                    itemName += (" " + SpecialInstructions);
                }

                return itemName.ToUpper();
            }
            set
            {
                ItemName = value;
            }
        }

        [Required]
        public int CompanyId { get; set; } // FK

        [Required]
        public int CustomerId { get; set; } // FK - inside this currency object also av.

        [Required]
        public int BagTypeId { get; set; } // FK - inside this productfinishingtype, uom, diamention objectalso av

        [Required]
        public int PolyCombId { get; set; } // FK - inside this color, poly-material and polycombitemcatbridges-Ienum objects also av.

        [Required]
        public int OrderUomId { get; set; } // FK

        public int? PrintTypeId { get; set; } //FK

        [Required]
        public int ShipmentTermId { get; set; } //FK

        public int? CylinderId { get; set; } //FK - this contains uom object as well

        [Required]
        public int MeasurementUomId { get; set; } //FK

        //[Required]
        //public int IcpId { get; set; } //FK

        public string? DeliveryAddress { get; set; }

        [Required, Range(0.000001, double.MaxValue, ErrorMessage = "Order qty must be larger than 0.")]
        public double OrderQty { get; set; }

        public string? SpecialInstructions { get; set; }

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "Guage must be larger than 0.")]
        public double Guage { get; set; }

        [Required]
        public double Gusset { get; set; }

        [Required]
        public double FlapWidth { get; set; }

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "Width must be larger than 0.")]
        public double Width { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public double BottomGusset { get; set; }

        [Required]
        public bool Printed { get; set; }

        [Required]
        public int NoOfColors { get; set; }//?ok?

        [Required]
        [Range(0, 2, ErrorMessage = "Printing sides count is between 0-2")]
        public int NoOfPrintingSides { get; set; } // 0 <= this <=2

        [Required]
        public bool AutoCylinderSelection { get; set; }

        public int BlockAvailability { get; set; } // 1 - New, 2 - Available, 3 - Balance Cut, 0 - Not Selected

        public double? Turns { get; set; }

        [Required]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Area is 0.0000. Please check, seems you are missing some diamentions. Like width, length, gusset etc.")]
        public double Area { get; set; }

        [Required]
        public double NoOfMeters { get; set; }

        [Required]
        public double PreCostingWastePercentage { get; set; }

        [Required, Range(0.0005, double.MaxValue)]
        public double Weight { get; set; }

        [Required]
        public double BlockCostTotal { get; set; }

        [Required]
        public double OtherCostTotal { get; set; }

        [Required]
        public double PerUnitSuggestedUnitPrice { get; set; } // Native Currency

        [Required]
        public double PerUnitAdjustableUnitPrice { get; set; } // Native Currency

        // Adding virtual variables
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("BagTypeId")]
        public virtual BagType? BagType { get; set; }

        [ForeignKey("PolyCombId")]
        public virtual PolyCombination? PolyCombination { get; set; }

        [ForeignKey("OrderUomId")]
        public virtual Uom? OrderUom { get; set; }

        [ForeignKey("PrintTypeId")]
        public virtual PrintType? PrintType { get; set; }

        [ForeignKey("ShipmentTermId")]
        public virtual ShipmentTerm? ShipmentTerm { get; set; }

        [ForeignKey("CylinderId")]
        public virtual Cylinder? Cylinder { get; set; }

        [ForeignKey("MeasurementUomId")]
        public virtual Uom? MeasurementUom { get; set; }
        [NotMapped]
        public virtual double? SelectedExchangeRate { get; set; }

    }
}
