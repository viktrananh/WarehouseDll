using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPDetailModelLink
    {
        public string WorkId { get; set; }
        public string  WorkIdSub { get; set; }
        public string ModelId { get; set; }
        public string CusModel { get; set; }
        public int Request { get; set; }
        public int Real { get; set; }
     
     
    }
}
