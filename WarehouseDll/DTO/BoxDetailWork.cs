using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO
{
    public class BoxDetailWork
    {
        public int Id { get; set; }
        public string BoxSerial { get; set; }
        public string WorkId { get; set; }
        public string PO { get; set; }
        public string ModelId { get; set; }
        public int Qty { get; set; }
    }
}
