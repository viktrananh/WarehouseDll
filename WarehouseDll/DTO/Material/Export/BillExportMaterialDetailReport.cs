using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Export
{
    public class BillExportMaterialDetailReport : BillExportMaterialDetail
    {
        public float QtyArise { get; set; }
        public int RealExportArise { get; set; }
        public BillExportMaterialDetailReport() { }
    }
}
