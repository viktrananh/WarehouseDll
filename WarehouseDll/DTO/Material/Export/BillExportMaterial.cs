using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Export
{
    public class BillExportMaterial
    {
        public static int BillSMT = 5;
        public static int BillPTH = 4;

        public string BillId { get; set; }
        public string ModelId { get; set; }
        public string CusId { get; set; }
        public string WorkId { get; set; }
        public int BillType { get; set; }
        public string BillTypeName { get; set; }
        public int ExportType { get; set; }
        public string ExportTypeName { get; set; }

        public int BillStatus { get; set; }
        public string BillStatusName { get; set; }

        public int Pcbs { get; set; }
        public DateTime CreateTime { get; set; }
        public string BomVersion { get; set; }
        public string OP { get; set; }
        public List<BillExportMaterialDetail> BillExportMaterialDetails { get; set; }

        public BillExportMaterial() { }

        public BillExportMaterial(DataRow row)
        {
            var subType = int.Parse(row["SUB_TYPE"].ToString());
            string loaiphieu = LoadSubTypeBillExportMaterial.SubTypeBillExportMaterial.FirstOrDefault(x => x.Id == subType).Name;
            int statusid = int.Parse(row["STATUS_EXPORT"].ToString());
            string statusName = LoadStatusBillExportMaterial.StatusBillExportMaterial.Where(a => a.Id == statusid).FirstOrDefault().Name;

            BillId = row["BILL_NUMBER"].ToString();
            WorkId = row["WORK_ID"].ToString();
            ModelId = row[""].ToString();
            CusId = row[""].ToString();
            BillType = subType;
            BillTypeName = row[""].ToString();
            ExportType = statusid;
            ExportTypeName = statusName;
            BillStatus = int.Parse(row[""].ToString());
            BillStatusName = row[""].ToString();
            Pcbs = int.Parse(row[""].ToString());
            CreateTime = DateTime.Parse(row["TIME_CREATE"].ToString());
            BomVersion = row["BOM_VERSION"].ToString();
            OP = row["OP"].ToString();
        }

    }
}
