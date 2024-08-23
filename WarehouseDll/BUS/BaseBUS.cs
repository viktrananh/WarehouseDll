using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.FinishedProduct;

namespace WarehouseDll.BUS
{
    public class BaseBUS
    {
        public static SqlClass _MySql = new SqlClass("mariadbsv1.mvn", "TRACKING_SYSTEM", "admin", "ManuAdmin$123");
        public static SqlClass _SqlSever = new SqlClass("192.168.10.253\\PANACIM", "PANACIM", "sa", "PANASONIC1!");

        public bool CreatFPBill(FPBill bill, string UserId, FPBillType billType, int state)
        {
            var dateReal = bill.IntendTime.Hour < 8 ? bill.IntendTime.AddDays(-1) : bill.IntendTime;

            string sql = $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILLS` (`BILL_NUMBER`, `CUS_ID`, `TIME`, `OP`, `TYPE_BILL`, `STATE`, `INTEND_TIME`, VEHICLE, TRUE_NUMBER) VALUES " +
             $"('{bill.BillNumber}', '{bill.CusId}', now(), '{UserId}', '{billType.Id}', '{state}', '{bill.IntendTime}', '{bill.Vehicle}' , '{bill.TrueNumber}');";
            foreach (var item in bill.FPBillDetailS)
            {
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILL_DETAILS` (`BILL_NUMBER`, `WORK_ID`,  `REQUEST`, `REAL` ,  `STATE_ID`, `CREAT_TIME` , `DATE_REAL`) VALUES " +
                     $" ('{bill.BillNumber}', '{item.WorkId}', '{item.Request}', '{item.Real}', '{item.StateId}', NOW() , `{dateReal.ToString("yyyy-MM-dd")}` ); ";
            }
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }
        public bool CreateBill(FPBill bill, string billNumber, string cusID, string timeExport, string vehicle, int trueNumber, string userID, FPBillType fPBillType)
        {
            string sql = string.Empty;
            int special = 0;

            foreach (var item in bill.FPBillDetailS)
            {
                string work = item.WorkId;
                string cus_model = item.CusModel;
                string cus_code = item.CusCode;
                string po = item.PO;
                int request = item.Request;
                string model = item.ModelId;
                sql += $"INSERT INTO TRACKING_SYSTEM.DELIVERY_BILL (BILL_NUMBER, WORK_ID, CUS_MODEL, CUS_CODE, UNIT, PO, NUMBER_REQUEST, MODEL,  STATUS_BILL,SPECIAL,DATE_EXPORTS , NOTE) VALUES " +
                     $"('{billNumber}', '{work}', '{cus_model}', '{cus_code}', 'PCS', '{po}', '{request}', '{model}', '{0}','{special}','{timeExport}' , '{item.Note}') ON DUPLICATE KEY UPDATE NUMBER_REQUEST = {request} ;";
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILL_DETAILS` (`BILL_NUMBER`, `WORK_ID`,  `REQUEST`, `STATE_ID`, `CREAT_TIME`) VALUES " +
                    $" ('{billNumber}', '{work}', '{request}', '0', NOW() ); ";
            }
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILLS` (`BILL_NUMBER`, `CUS_ID`, `TIME`, `OP`, `TYPE_BILL`, `NOTE`, `INTEND_TIME`, VEHICLE, TRUE_NUMBER) VALUES " +
             $"('{billNumber}', '{cusID}', now(), '{userID}', '{fPBillType.Id}', '1',  '{timeExport}', '{vehicle}' , '{trueNumber}');";

            if (!_MySql.InsertDataMySQL(sql)) return false;
            return true;

        }


    }
}
