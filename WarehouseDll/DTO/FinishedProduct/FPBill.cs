using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO.FinishedProduct;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPBill
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string CusId { get; set; }
        public DateTime CreatTime { get; set; }
        public string OP { get; set; }
        public int TypeBillId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public DateTime IntendTime { get; set; }
        public string Note { get; set; }
        public string Vehicle { get; set; }
        public int TrueNumber { get; set; }
        public int IsBook { get; set; }
        public List<FPBillDetail> FPBillDetailS { get; set; }
        public FPBill() { }
    
        public FPBill(DataRow row, bool notDetail = true)
        {
            Id = int.Parse(row["ID"].ToString());
            BillNumber = row["BILL_NUMBER"].ToString();
            CusId = row["CUS_ID"].ToString();
            CreatTime = DateTime.Parse(row["TIME"].ToString());
            OP = row["OP"].ToString();
            TypeBillId = int.Parse(row["TYPE_BILL"].ToString());
            StateId = int.Parse(row["STATE"].ToString());
            StateName = LoadStateBillExportGoodsToCus.StatusBillExportGoodToCus.FirstOrDefault(x=>x.Id == StateId).Name;
            IntendTime = string.IsNullOrEmpty(row["INTEND_TIME"].ToString()) ? CreatTime : DateTime.Parse(row["INTEND_TIME"].ToString());
            Note = row["NOTE"].ToString();
            IsBook = int.Parse(row["IS_BOOK"].ToString());
            FPBillDetailS = notDetail ? new List<FPBillDetail>() :  new FPBillExportDAO().GetBillDetailByBillId(BillNumber, notDetail);
        }
    }
}
