using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.SangChai.WgRecords.Shell.Models
{
    public class MeasuringData
    {
        public decimal TotalNetWeight { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }

    public class MeasuringItemData : WeightData
    {
        public Guid Id { get; set; }
        public long SeqNo { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool CurrentClipboard { get; set; }
    }

    public class WeightData
    {
        public decimal NetWeight { get; set; }
        public decimal TareWeight { get; set; }
    }

    public class InfoData : EventArgs
    {
        public string Text { get; set; }
    }
}
