using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPWorkRemain
    {
        public int Id { get; set; }
        public string WorkId { get; set; }
        public int Remain { get; set; }
        public int Imp { get; set; }
        public int Exp { get; set; }
        public int ImpLocal { get; set; }
        public int ImpCus { get; set; }
        public int ExpCus { get; set; }
        public int ExpLocal { get; set; }

    }
}
