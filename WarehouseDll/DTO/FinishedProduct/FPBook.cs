using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPBook
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string WorkId { get; set; }
        public int Request { get; set; }
        public string BoxSerial { get; set; }
        public int BoxCount { get; set; }
        public int RequestAfter { get; set; }
        public string LotNo { get; set; }
        public int StateId { get; set; }

    }
}
