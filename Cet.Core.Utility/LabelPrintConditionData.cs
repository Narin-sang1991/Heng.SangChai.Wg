using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Cet.Core.Utility
{
    [KnownType(typeof(StickerBarcodeData))]
    [KnownType(typeof(ShelfLabelData))]
    [KnownType(typeof(PromotionLabelData))]
    public class LabelPrintConditionData
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public decimal? ProductPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class StickerBarcodeData : LabelPrintConditionData
    {
        public bool ShowPriceOption { get; set; }
    }

    public class ShelfLabelData : LabelPrintConditionData
    {

    }

    public class PromotionLabelData : LabelPrintConditionData
    {
        public decimal PromotionPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AdditionalCondition { get; set; }
    }
}
