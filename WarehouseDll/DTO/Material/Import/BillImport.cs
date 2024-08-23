using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO.Material;

namespace WarehouseDll.DTO.Material.Import
{
    public class BillImport : Bill
    {
        public int PN_NVL_TU_Khach = 2;
        public int PN_NVL_TU_SX = 3;
        public int PN_CCDC_TU_Khach = 1;
        public int state_Fail = 4;
        public int state_Pass = 6;
        public int state_Ongoing = 3;
        public bool IsImportCusPart { get; set; }

        public int qty_recived { get; set; }
        public string StateName { get; set; }
        public List<BillImportInfor> BillImportInfors { get; set; }
        public List<ROSE_Dll.DTO.ManuMasterPart> PartsInfor { get; set; }
        public List<PartSpecialEXP> PartSpecialEXPs { get; set; }
        public List<BillImportPO> BillImportPOs { get; set; }
        public BillImport() { }
        public BillImport(DataRow row , bool loadDetail = false)
        {
            BillNumber = row["BILL_NUMBER"].ToString();
            WorkId = row["WORK_ID"].ToString();
            CusId = row["CUSTOMER"].ToString();
            CreatTime = DateTime.Parse( row["CREATE_TIME"].ToString());
            StateName = row["STATUS"].ToString();
            StateId = int.Parse( row["STATUS_BILL"].ToString());
            OP = row["CREATE_BY"].ToString();
            VenderId = row["VENDER_ID"].ToString();
            VenderName = row["VENDER_NAME"].ToString();

            //BillImportInfors = loadDetail ? new BillImportDAO().GetListPart(BillNumber).OrderByDescending(x=>x.ReciveQty).ToList() : new List<BillImportInfor>() ;
            BillImportPOs = loadDetail ? new BillImportDAO().GetBillImportPO(BillNumber) : new List<BillImportPO>();
        }


    }
}
