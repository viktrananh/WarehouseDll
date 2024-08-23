using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class DataInput
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string CusId { get; set; }
        public string DID { get; set; }
        public string PartNumber { get; set; }
        public string Qty { get; set; }
        public DateTime TimeInput   { get; set; }
        public string CusPart { get; set; }
        public string MFGPart { get; set; }
        public string DateCode { get; set; }
        public string LotCode { get; set; }
        public string Machine { get; set; }
        public string DateEXP { get; set; }
        public string DateEXPReal { get; set; }
        public string OP { get; set; }
        public int IsBlock { get; set; }
        public int IsESD { get; set; }
        public int REPrint { get; set; }
        public int Special { get; set; }
        public string VenderPart { get; set; }
        public string VenderName { get; set; }
        public int DefineExport { get; set; }
        public string Buyer { get; set; }
        public int IQCResult { get; set; }
        public string IQCIndex { get; set; }
    }
}
