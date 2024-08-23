using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class BoxReportLocation
    {
        public string BoxSerial { get; set; }
        public int Qty { get; set; }
        public string Location { get; set; }
        public DateTime TimeImport { get; set; }
        public DateTime TimePacking { get; set; }
        public bool IsStogareTime { get; set; } = true;

        public BoxInfor BoxInfor { get; set; }
    }
}
