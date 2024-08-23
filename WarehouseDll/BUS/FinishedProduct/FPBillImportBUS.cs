using MySqlX.XDevAPI;
using ROSE_Dll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO;
using WarehouseDll.DTO.FinishedProduct;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.BUS.FinishedProduct
{
    public class FPBillImportBUS : BaseBUS
    {
        public bool CreateBillImport(List<FPLocation> fPLocation, string billID, string cusId, int firstRemain, string process, bool IsYesterday,
           string OP, int TYPE_BILL, int remianSFP)
        {
            string clause = string.Empty;
            if (!IsYesterday) clause = " CURDATE() ";
            else clause = " ADDDATE( CURDATE(), -1) ";
            string sql = string.Empty;

            int sumWork = fPLocation.Sum(x => x.Qty);
            string workId = fPLocation.FirstOrDefault().WorkId;
            string modelId = fPLocation.FirstOrDefault().BoxInfor.Model;

            foreach (var item in fPLocation)
            {
                var boxInfor = item.BoxInfor;

                string boxSeri = item.BoxSerial;
                string location = item.Location;
                string work = item.WorkId;
                string model = boxInfor.Model;
                string timePacked = boxInfor.TimePacking.ToString("yyyy-MM-dd HH:mm:ss");

                int pcbs = item.Qty;
                //thành phẩm

                int level = item.BoxInfor.Level;
                if (level != 1)
                {
                    foreach (var childen in item.BoxInfor.BoxChilden)
                    {
                        sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = '1' WHERE BOX_SERIAL = '{childen.BoxSerial}';";

                    }
                }
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_LOCATION` (`BOX_SERIAL`, `LOCATION`, `WORK_ID`, `CREAT_TIME`, `OP`, `BILL_NUMBER`, `QTY`) VALUES " +
                    $" ('{boxSeri}', '{location}', '{work}', now(), '{OP}', '{billID}', '{boxInfor.Qty}');";
                if(firstRemain == 0)
                {
                    sql += $"INSERT INTO `TRACKING_SYSTEM`.`FB_BOX_HISTORY` (`BILL_NUMBER`, `WORK_ID`, `BOX_SERIAL`, `QTY_BEFORE`, `QTY`, `QTY_AFTER`, `EVENT_TIME`) VALUES " +
                   $" ('{billID}', '{work}', '{boxSeri}', 0, '{pcbs}', {pcbs}, now());";
                }
                else
                {
                    sql += $"INSERT INTO `TRACKING_SYSTEM`.`FB_BOX_HISTORY` (`BILL_NUMBER`, `WORK_ID`, `BOX_SERIAL`, `QTY_BEFORE`, `QTY`, `QTY_AFTER`, `EVENT_TIME`) VALUES " +
                   $" ('{billID}', '{work}', '{boxSeri}', (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN WHERE WORK_ID ='{work}'), '{pcbs}', (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN WHERE WORK_ID ='{work}') +{pcbs}, now());";
                }
               
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_WORK_REMAIN` (`WORK_ID`, `REMAIN`, `IMP`, `IMP_LOCAL`) VALUES " +
                    $" ('{work}', '{pcbs}', '{pcbs}', '{pcbs}') ON DUPLICATE KEY UPDATE `REMAIN` = `REMAIN` + {pcbs} , `IMP` = `IMP` + {pcbs}, `IMP_LOCAL` = `IMP_LOCAL` + {pcbs} ;";
                sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = '1' WHERE BOX_SERIAL = '{boxSeri}';";
            }
            sql += "INSERT INTO `TRACKING_SYSTEM`.`FP_BILLS` (`BILL_NUMBER`, `CUS_ID`, `TIME`, `OP`, `TYPE_BILL`, `STATE`, `NOTE`) VALUES " +
              $"('{billID}', '{cusId}', NOW(), '{OP}', '{TYPE_BILL}', '{3}', '{process}-TP');";
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_DAY_STOCK_TRACKING` (`WORK_ID`,  `DATE`, `FIRST_REMAIN`, `IMPORT`, `LAST_REMAIN`) VALUES" +
         $" ('{workId}',  {clause}, '{firstRemain}', '{sumWork}', '{firstRemain + sumWork}') ON DUPLICATE KEY UPDATE `IMPORT` =  `IMPORT` + {sumWork}, `LAST_REMAIN` = `LAST_REMAIN` + {sumWork} ; ";
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILL_DETAILS` (`BILL_NUMBER`, `WORK_ID`, `REQUEST`, `REAL`, `STATE_ID`, `OP`, `CREAT_TIME`, `COMPLETION_TIME`, `DATE_REAL`) VALUES " +
                $" ('{billID}', '{workId}', '{sumWork}', '{sumWork}', '1', '{OP}',NOW(), NOW() , {clause});";


            //-----------------------------------------------------Bán thành phẩm-----------------------------------------------------------
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`SFP_BILLS` (`BILL_ID`, `MODEL`, `WORK_ID`, `LOCATION`," +
                $" `QTY`, `QTY1`, `QTY2`, `RECIVER`, `EVENT_TIME`, `NOTE`) VALUES " +
               $"('{billID}', '{modelId}', '{workId}', '{process}'," +
                 $" '{sumWork}', {remianSFP}, " +
                 $" {remianSFP} -{sumWork} , '{OP}', now(), '{process}-TP');";

            sql += $"INSERT INTO `TRACKING_SYSTEM`.`SFP_REMAIN_CURRENT` (`MODEL`, `WORK_NUMBER`, `QTY`, `LOCATION`) " +
                $"VALUES ('{modelId}', '{workId}', {remianSFP} - {sumWork}, '{process}') ON DUPLICATE KEY UPDATE  QTY = {remianSFP} - {sumWork};";



            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }



        public bool CreatBillImportFromCus(List<FPLocation> fPLocation, FPBill fPBillExported, string user, string userConfirm, string billRe, string clause)
        {
            string sql = string.Empty;
            int sumPCB = fPLocation.Sum(x => x.Qty);

            string workId = fPLocation.FirstOrDefault().WorkId;
            string modelId = fPLocation.FirstOrDefault().BoxInfor.Model;

            foreach (var item in fPLocation)
            {
                var boxInfor = item.BoxInfor;

                string boxSeri = item.BoxSerial;
                string location = item.Location;
                string work = item.WorkId;
                string model = boxInfor.Model;
                string timePacked = boxInfor.TimePacking.ToString("yyyy-MM-dd HH:mm:ss");

                int pcbs = item.Qty;
                //thành phẩm

                int level = item.BoxInfor.Level;
                if (level != 1)
                {
                    foreach (var childen in item.BoxInfor.BoxChilden)
                    {
                        sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = '1' WHERE BOX_SERIAL = '{childen.BoxSerial}';";

                    }
                }
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_LOCATION` (`BOX_SERIAL`, `LOCATION`, `WORK_ID`, `CREAT_TIME`, `OP`, `BILL_NUMBER`, `QTY`) VALUES " +
                    $" ('{boxSeri}', '{location}', '{work}', now(), '{user}', '{billRe}', '{boxInfor.Qty}');";

                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FB_BOX_HISTORY` (`BILL_NUMBER`, `WORK_ID`, `BOX_SERIAL`, `QTY_BEFORE`, `QTY`, `QTY_AFTER`, `EVENT_TIME`) VALUES " +
                  $" ('{billRe}', '{work}', '{boxSeri}', (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN WHERE WORK_ID ='{work}'), '{pcbs}', (SELECT SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN WHERE WORK_ID ='{work}') +{pcbs}, now());";
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_WORK_REMAIN` (`WORK_ID`, `REMAIN`, `IMP`, `IMP_CUS`) VALUES " +
                    $" ('{work}', '{pcbs}', '{pcbs}', '{pcbs}') ON DUPLICATE KEY UPDATE `REMAIN` = `REMAIN` + {pcbs} , `IMP` = `IMP` + {pcbs}, `IMP_CUS` = `IMP_CUS` + {pcbs} , `EXP_CUS` = `EXP_CUS` - {pcbs};";

                //22/2/2024 Huyền
                sql += $"UPDATE TRACKING_SYSTEM.WORK_ORDER SET EXPORT=EXPORT-{item.Qty},STATUS='' WHERE WORK_ID='{item.WorkId}';";

                sql += $"UPDATE TRACKING_SYSTEM.BOX_LIST SET STATE = 1 where BOX_SERIAL='{item.BoxSerial}' ; ";
                sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_DAY_STOCK_TRACKING` (`WORK_ID`, `MODEL_ID`, `DATE`, `FIRST_REMAIN`, `IMPORT`, `LAST_REMAIN`) VALUES" +
                        $" ('{work}', '{model}', {clause}, (SELECT  SUM(REMAIN) FROM TRACKING_SYSTEM.FP_WORK_REMAIN where WORK_ID='{work}'), '{item.Qty}',  (SELECT REMAIN FROM TRACKING_SYSTEM.FP_WORK_REMAIN where WORK_ID='{item.WorkId}') + {item.Qty}) " +
                        $" ON DUPLICATE KEY UPDATE `IMPORT` =  `IMPORT` + {item.Qty}, `LAST_REMAIN` = `LAST_REMAIN` + {item.Qty} ; ";
                sql += $"DELETE FROM TRACKING_SYSTEM.FB_BOX_HISTORY where BILL_NUMBER='{fPBillExported.BillNumber}' and work_id = '{workId}'  AND BOX_SERIAL = '{boxSeri}';";
            }
            sql += $"UPDATE TRACKING_SYSTEM.FP_BILLS SET STATE = 2 where BILL_NUMBER='{fPBillExported.BillNumber}';";
            sql += $"UPDATE TRACKING_SYSTEM.FP_BILL_DETAILS SET STATE_ID=0 , `REAL` = `REAL` - {sumPCB} where BILL_NUMBER='{fPBillExported.BillNumber}' AND WORK_ID='{workId}';";
            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILLS` (`BILL_NUMBER`, `CUS_ID`, `TIME`, `OP`, `CONFIRM`, `TYPE_BILL`, `STATE`, `NOTE`) " +
               $" VALUES ('{billRe}', '{fPBillExported.CusId}', NOW(), '{user}', '{userConfirm}', '15', '1', '{fPBillExported.BillNumber}');";

            sql += $"INSERT INTO `TRACKING_SYSTEM`.`FP_BILL_DETAILS` (`BILL_NUMBER`, `WORK_ID`, `REQUEST`, `REAL`, `STATE_ID`, `OP`, `CREAT_TIME`, `COMPLETION_TIME` , `DATE_REAL`) VALUES " +
                $" ('{billRe}', '{workId}', '{sumPCB}', '{sumPCB}', '1', '{user}',NOW(), NOW() , {clause});";
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;

        }
    }
}
