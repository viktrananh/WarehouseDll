using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class Location
    {

        public string LocationName { get; set; }
        public string WorkId { get; set; }
        public string ModelId { get; set; }

        public List<BoxReportLocation> Boxs { get; set; }
        public int BoxNumber { get; set; }
        public int PcbNumber { get; set; }
        public string CusID { get; set; }
        public Location() { }

    }
}
