using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class BillExecuteDetail
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string  Part { get; set; }
        public string PartNumber { get; set; }
        public string CusPart { get; set; }
        public string DID { get; set; }
        public int Qty { get; set; }
    }
}
