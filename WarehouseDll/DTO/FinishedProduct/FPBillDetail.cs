using ROSE_Dll.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO;
using WarehouseDll.DAO.FinishedProduct;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPBillDetail
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string WorkId { get; set; }
        public string PO { get; set; }
        public string ModelId { get; set; }
        public string CusModel { get; set; }
        public string CusCode { get; set; }
        public int Request { get; set; }
        public int Real { get; set; }
        public string OP { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public DateTime? CreatTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public DateTime?  DatePacking { get; set; }
        public int BoxCarton { get; set; }
        public int BoxPlastic { get; set; }
        public string PackInfor { get; set; }
        public string Note { get; set; }
        public List<FPBoxHistory> FPBoxHistories { get; set; }

        public FPBillDetail() { }
        public FPBillDetail(DataRow row, bool notDetail = true)
        {
            string work = row["WORK_ID"].ToString();
            bool IsModelLink = new BaseDAO().IsWorkSEFLinkModel(work);
            string po = row["PO"].ToString();
            Id = int.Parse(row["ID"].ToString());
            BillNumber = row["BILL_NUMBER"].ToString();
            WorkId = work;
            PO = po;
            ModelId = row["ID_MODEL"].ToString();
            CusModel = row["MODEL_NAME"].ToString();    
            Request = int.Parse( row["REQUEST"].ToString());
            Real = int.Parse(row["REAL"].ToString());
            OP = row["OP"].ToString();
            Note = row["NOTE"].ToString();
            StateId = int.Parse(row["STATE_ID"].ToString());
            StateName = LoadStateBillDetailExportGoodToCus.StateBilDetailExportGoodToCus.FirstOrDefault(x => x.Id == StateId).Name;
            CreatTime = string.IsNullOrEmpty(row["CREAT_TIME"].ToString()) ? DateTime.MinValue:  DateTime.Parse(row["CREAT_TIME"].ToString());
            CompletionTime = !string.IsNullOrEmpty(row["COMPLETION_TIME"].ToString()) ? DateTime.Parse(row["COMPLETION_TIME"].ToString()) : !string.IsNullOrEmpty(row["CREAT_TIME"].ToString()) ? DateTime.Parse(row["CREAT_TIME"].ToString()) : DateTime.MinValue;
            FPBoxHistories = notDetail ? new List<FPBoxHistory>() : new FPBillExportDAO().GetFPBoxHistory(BillNumber, work, IsModelLink);

            var boxDetailWork = FPBoxHistories.SelectMany(x => x.BoxDetailWorks).ToList() ;
            if(!new FPBillExportDAO().IsListEmty(boxDetailWork))
            {
                var listPo = (from r in boxDetailWork
                              group r by r.PO into gr
                              select gr.Key).ToList();
                string poExtend = string.Join($"\n", listPo);
                PO = $"{poExtend}";
            }
            DatePacking = new DateTime();
            var groups = FPBoxHistories.Select(x=>x.Qty).GroupBy(a=>a);
            string packInfor =string.Empty;
            foreach (var group in groups)
                packInfor += group.Count() + $" Thùng ( {group.Key} PCS/Thùng )\n";
            if (string.IsNullOrEmpty(packInfor))
            {
                packInfor = "       Thùng (     PCS/Thùng )\n       Thùng (     PCS/Thùng )  ";
            }
            PackInfor =   packInfor;
            Note = packInfor;
            BoxPlastic = int.Parse(row["BOX_P"].ToString());
            BoxCarton  = int.Parse(row["BOX_C"].ToString());
        }
    }
}
