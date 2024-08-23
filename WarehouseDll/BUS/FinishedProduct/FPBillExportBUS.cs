using MySqlX.XDevAPI;
using ROSE_Dll.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO;
using WarehouseDll.DTO.FinishedProduct;
using static System.Windows.Forms.AxHost;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.BUS.FinishedProduct
{
    public class FPBillExportBUS : BaseBUS
    {
       

    
        public bool UpdateStateBillDetail (string bill, string workId, int stateId)
        {
            string sql = $"UPDATE TRACKING_SYSTEM.FP_BILL_DETAILS SET STATE_ID = '{stateId}' WHERE BILL_NUMBER= '{bill}' AND WORK_ID = '{workId}'; ";
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }
        public bool ConfirmFPBillDetail(string bill, string workId, int stateId, int boxC, int boxP, string userId, string dateReal)
        {
            string sql = $"UPDATE TRACKING_SYSTEM.FP_BILL_DETAILS SET STATE_ID = '{stateId}', `OP` = '{userId}' , BOX_C = {boxC},BOX_P = {boxP}, DATE_REAL = '{dateReal}' WHERE BILL_NUMBER= '{bill}' AND WORK_ID = '{workId}'; ";
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }
        public bool UpdateStateFPBill(string bill, int stateId)
        {
            string sql = $"UPDATE TRACKING_SYSTEM.FP_BILLS SET STATE = '{stateId}' WHERE BILL_NUMBER= '{bill}' ; ";
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }

        public bool FinishFPBill(FPBill fPBill)
        {
            string sql = "";
            sql += $"UPDATE TRACKING_SYSTEM.FP_BILLS SET STATE = '{LoadStateBillExportGoodsToCus.COMPLETE}' WHERE BILL_NUMBER= '{fPBill.BillNumber}' ; ";
            foreach (var item in fPBill.FPBillDetailS)
            {
                sql += $"UPDATE TRACKING_SYSTEM.FP_BILL_DETAILS SET STATE_ID = '{1}' WHERE BILL_NUMBER= '{item.BillNumber}' ; ";

            }
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }

        public bool ExportBox(BoxInfor boxinfor, FPBillDetail fd, int firstRemain, bool IsYesterday,  string boxCus = "", int IsBook = 0)
        {
            string clause = string.Empty;
            if (!IsYesterday) clause = " CURDATE() ";
            else clause = " ADDDATE( CURDATE(), -1) ";

            string po = string.IsNullOrEmpty(boxCus) ? string.Empty : boxCus.Split(',').Last();

            string sql = string.Empty;
            int boxCount = boxinfor.Qty;
             sql += $"INSERT INTO `TRACKING_SYSTEM`.`FB_BOX_HISTORY` (`BILL_NUMBER`, `WORK_ID`, `MODEL_ID`, `BOX_SERIAL`, `QTY_BEFORE`, `QTY`, `QTY_AFTER`, `EVENT_TIME`) " +
                $" VALUES ('{fd.BillNumber}', '{fd.WorkId}', '{fd.ModelId}',  '{boxinfor.BoxSerial}', '{firstRemain}', '{boxCount}', '{firstRemain - boxCount}', NOW() );";
            sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = 2, BOX_CUS = '{boxCus}', PO = '{po}' WHERE BOX_SERIAL = '{boxinfor.BoxSerial}';";
            if (boxinfor.Level > 1)
            {
                foreach (var item in boxinfor.BoxChilden)
                {
                    sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = 2, PO = '{po}' WHERE BOX_SERIAL = '{item.BoxSerial}';";
                }
            }
            sql += $"delete from TRACKING_SYSTEM.FP_LOCATION where BOX_SERIAL = '{boxinfor.BoxSerial}' ;";
            //22-2-2024 Huyền
            sql += $"UPDATE TRACKING_SYSTEM.WORK_ORDER SET EXPORT=EXPORT+{boxCount} WHERE WORK_ID='{fd.WorkId}';";

            sql += $"UPDATE TRACKING_SYSTEM.WORK_ORDER SET STATUS='CLOSE' WHERE WORK_ID='{fd.WorkId}' AND EXPORT=PCBS;";

            sql += $"UPDATE TRACKING_SYSTEM.FP_WORK_REMAIN SET EXP = EXP + {boxCount} , EXP_CUS = EXP_CUS +  {boxCount}, REMAIN  = REMAIN - {boxCount} WHERE WORK_ID = '{fd.WorkId}';";

            //---------------------------------------------------
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_DAY_STOCK_TRACKING` (`WORK_ID`, `MODEL_ID`, `DATE`, `FIRST_REMAIN`, `EXPORT`, `LAST_REMAIN`) " +
                $"VALUES ('{fd.WorkId}', '{fd.ModelId}',   {clause} , '{firstRemain}', '{boxCount}', '{firstRemain - boxCount}') ON DUPLICATE KEY UPDATE `EXPORT` = `EXPORT` + {boxCount},  `LAST_REMAIN` = `LAST_REMAIN` - {boxCount} ;";
            sql += $"UPDATE TRACKING_SYSTEM.FP_BILL_DETAILS SET `REAL` = `REAL` + {boxinfor.Qty} where bill_number='{fd.BillNumber}' AND WORK_ID='{fd.WorkId}';";
            if (IsBook == 1)
            {
                sql += $"UPDATE TRACKING_SYSTEM.FP_BOOK_BILL_DETAIL SET STATE = 1 WHERE BILL_NUMBER = '{fd.BillNumber}' AND BOX_SERIAL = '{boxinfor.BoxSerial}';";
            }
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }

        public bool CreatBillReturnProduction(List<FPLocation> boxRetruns, string bill, string workEx, string modelID, string cusModel, string cusID, int firstRemain,
           string _userID, string TYPE_BILL, string PROCESS, bool IsYesterday, string op, int remainProcess)
        {
            string clause = string.Empty;
            if (!IsYesterday) clause = " CURDATE() ";
            else clause = " ADDDATE( CURDATE(), -1) ";
            int sumPCB = boxRetruns.Sum(x => x.Qty);
            string sql = string.Empty;
            foreach (var item in boxRetruns)
            {

                string boxID = item.BoxSerial;
                int count = item.Qty;
                string location = item.Location;

                //--------Thành phẩm


                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FB_BOX_HISTORY` (`BILL_NUMBER`, `WORK_ID`, `BOX_SERIAL`, `QTY_BEFORE`, `QTY`, `QTY_AFTER`, `EVENT_TIME`) " +
                    $" VALUES ('{bill}', '{workEx}','{boxID}',  (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN where WORK_ID= '{workEx}'), '{count}', (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN where WORK_ID= '{workEx}') - {count}, now());";
                sql += $"UPDATE TRACKING_SYSTEM.FP_WORK_REMAIN SET REMAIN = REMAIN - {count}, EXP = EXP + {count}, EXP_LOCAL = EXP_LOCAL + {count}, IMP_LOCAL = IMP_LOCAL - {count} WHERE WORK_ID = '{workEx}';";

                sql += $"DELETE FROM TRACKING_SYSTEM.FP_LOCATION WHERE BOX_SERIAL = '{boxID}';";

                sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = '{0}' WHERE BOX_SERIAL = '{boxID}';";
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_DAY_STOCK_TRACKING` (`WORK_ID`, `MODEL_ID`, `CUS_MODEL`, `CUS_ID`, `DATE`, `FIRST_REMAIN`, `EXPORT`, `LAST_REMAIN`) " +
                        $"VALUES ('{workEx}', '{modelID}', '{cusModel}', '{cusID}', {clause} , '{firstRemain}', '{count}', '{firstRemain - count}') " +
                        $"  ON DUPLICATE KEY UPDATE `EXPORT` = `EXPORT` + {count},  `LAST_REMAIN` = `LAST_REMAIN` - {count} ;";
            }

            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILLS` (`BILL_NUMBER`, `CUS_ID`, `TIME`, `OP`, `TYPE_BILL`, `STATE`, `NOTE`) VALUES " +
                $"('{bill}', '{cusID}', now(), '{op}', '{TYPE_BILL}', '3', 'TP-{PROCESS}');";

            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILL_DETAILS` (`BILL_NUMBER`, `WORK_ID`, `REQUEST`, `REAL`, `STATE_ID`, `OP`, `CREAT_TIME`, `COMPLETION_TIME`, `DATE_REAL`) VALUES " +
                $" ('{bill}', '{workEx}', '{sumPCB}', '{sumPCB}', '1', '{op}', now(), now(), {clause})  ;";
            //------------------------Bán thành phẩm-------------------------
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`SFP_BILLS` (`BILL_ID`, `MODEL`, `WORK_ID`, `LOCATION`," +
                   $" `QTY`, `QTY1`, `QTY2`, `RECIVER`, `EVENT_TIME`, `NOTE`) VALUES " +
                     $"('{bill}', '{modelID}', '{workEx}', '{PROCESS}'," +
                     $" '{sumPCB}', {remainProcess},  {remainProcess} + {sumPCB} ," +
                     $" '{_userID}', now(), 'TP-{PROCESS}');";
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`SFP_REMAIN_CURRENT` (`MODEL`, `WORK_NUMBER`, `QTY`, `LOCATION`, `INIT_TIME`) VALUES ('{modelID}', '{workEx}', '{sumPCB}', '{PROCESS}', now()) ON DUPLICATE KEY UPDATE  QTY = {remainProcess} + {sumPCB}  ;";
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;

        }
    }
}
