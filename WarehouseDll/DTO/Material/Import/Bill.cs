using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class Bill
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string WorkId { get; set; }
        public string ModelId { get; set; }
        public string CusId { get; set; }
        public string Warehouse { get; set; }
        public int TypeBill { get; set; }
        public int SubType { get; set; }
        public int StateId { get; set; }
        public DateTime IntendTime { get; set; }
        public string OP { get; set; }
        public DateTime CreatTime { get; set; }
        public string VenderId { get; set; }
        public string VenderName { get; set; }
        public string Note { get; set; }
        public string PO { get; set; }
        public int DefineBill { get; set; }
        public Bill() { }
        public Bill(DataRow row)
        {
            Id = int.Parse(row["ID"].ToString());
            BillNumber = row["BILL_NUMBER"].ToString();
            WorkId = row["WORK_ID"].ToString();
            ModelId = row["MODEL_ID"].ToString();
            CusId = row["CUS_ID"].ToString();
            Warehouse = row["WAREHOUSE"].ToString();
            OP = row["OP"].ToString();
            TypeBill = int.Parse(row["TYPE_BILL"].ToString());
            SubType = int.Parse(row["SUB_TYPE"].ToString());
            StateId = int.Parse(row["STATE_ID"].ToString());
            CreatTime = DateTime.Parse(row["CREAT_TIME"].ToString());
            IntendTime = DateTime.Parse(row["INTEND_TIME"].ToString());
        }
    }
}
