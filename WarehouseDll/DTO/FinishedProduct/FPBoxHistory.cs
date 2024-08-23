using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPBoxHistory
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string WorkId { get; set; }
        public string ModelId { get; set; }
        public string CusId { get; set; }
        public string BoxSerial { get; set; }
        public int QtyBefore { get; set; }
        public int Qty { get; set; }
        public int QtyAfter { get; set; }
        public DateTime DatePacking { get; set; }
        public DateTime EventTime { get; set; }

        public List<BoxDetailWork> BoxDetailWorks { get; set; }
    }
}
