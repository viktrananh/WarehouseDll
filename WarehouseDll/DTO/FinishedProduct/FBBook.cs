using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FBBook
    {
        public int Id { get; set; }
        public string WorkId { get; set; }
        public int? Request { get; set; }
        public int Qty { get; set; }
        public int RequestAfter { get; set; }
        public string BoxSerial { get; set; }
        public string LotNo { get; set; }
        public string BillNumber { get; set; }
        public int State { get; set; }

    }
}
