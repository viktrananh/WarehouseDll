using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class BillImportPO
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string PO { get; set; }
        public string CusModel { get; set; }
        public string ModelId { get; set; }
        public DateTime CreatTime { get; set; }
    }
}
