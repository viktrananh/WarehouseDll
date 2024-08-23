using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class BillInfor
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string Part { get; set; }
        public int Qty { get; set; }
        public int ReciveQty { get; set; }
        public string Unit { get; set; }
    }
}
