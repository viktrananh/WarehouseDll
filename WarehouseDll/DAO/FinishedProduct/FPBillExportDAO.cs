using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.FinishedProduct;

namespace WarehouseDll.DAO.FinishedProduct
{
    public class FPBillExportDAO : BaseDAO
    {
        public string CreateFPBillExportCus(DateTime dtExport)
        {
            string billRequest = string.Empty;
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_BILLS where date(INTEND_TIME) =  date('{dtExport.ToString("yyyy-MM-dd HH:mm:ss")}') AND TYPE_BILL = 6 ORDER BY BILL_NUMBER DESC;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt))
            {
                billRequest = dtExport.ToString("ddMMyy") + "-01/TP";
            }
            else
            {
                billRequest = dtExport.ToString("ddMMyy") + $"-{(int.Parse(dt.Rows[0]["BILL_NUMBER"].ToString().Split('-')[1].Split('/')[0]) + 1).ToString("00")}" + "/TP";
            }
            return billRequest;
        }
        public string CreateBillReturnProduction(string process)
        {
            DateTime timeNow = GetTimeServer();
            string bill = $"{timeNow.ToString("ddMMyyyyHHmmss")}/TP-{process}";
            return bill;
        }
        public static string getDetailNote(string bill, string work, string cusCode, bool isVLS)
        {
            BaseDAO baseDAO = new BaseDAO();
            string note = "";
            List<int> listCout = new List<int>();
            DataTable dataTable = new DataTable();
            if (isVLS)
                dataTable =_MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.FB_BOX_HISTORY where BILL_NUMBER = '{bill}' and WORK_ID = '{work}' ;");
            else
                dataTable = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.FB_BOX_HISTORY WHERE BILL_NUMBER = '{bill}' AND WORK_ID = '{work}';");

            string[] arrPcbs = dataTable.AsEnumerable().Select(r => r.Field<string>("PCBS")).ToArray();

            var groups = arrPcbs.GroupBy(v => v);
            foreach (var group in groups)
                note += group.Count() + $"  Thùng ( {group.Key} PCS/Thùng ) \r\n";

            return note;
        }
        public string GetDetailNote(List<int> arrPcbs)
        {
            string note = "";
            var groups = arrPcbs.GroupBy(v => v);
            foreach (var group in groups)
                note += group.Count() + $"  Thùng ( {group.Key} PCS/Thùng ) \r\n";

            return note;
        }
    }
}
