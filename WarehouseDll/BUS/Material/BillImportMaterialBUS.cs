using ROSE_Dll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.BUS.Material.Import
{
    public class BillImportMaterialBUS : BaseBUS
    {
        public bool CreatBillImport(BillImport bill, string userId)
        {
            string sql = "INSERT INTO STORE_MATERIAL_DB.BILL_REQUEST_IMPORT (BILL_NUMBER, CUSTOMER, CREATE_BY, CREATE_TIME, INTEND_TIME, STATUS_BILL, TYPE_BILL, `VENDER_ID`,`PO`,`model_id`,`CUS_ID`,`WORK_ID`,`DEFINE_EXPORT`)" +
                      $" VALUE ('{bill.BillNumber}', '{bill.CusId}',  '{userId}',now(), now(), {1}, '{2}', '{bill.VenderId}','{bill.PO}','{bill.ModelId}','{bill.CusId}','{bill.WorkId}','{bill.DefineBill}');";

            sql += $"INSERT INTO `STORE_MATERIAL_DB`.`BILL` (`BILL_NUMBER`, `CUS_ID`, `WAREHOUSE`, `TYPE_BILL`, `SUB_TYPE`, `STATE_ID`, " +
                $"`INTEND_TIME`, `OP`, `CREAT_TIME`, `VENDER_ID`, `PO`, `DEFINE_BILL`) VALUES " +
                $" ('{bill.BillNumber}', '{bill.CusId}', 'WAREHOUSE', '{2}', '{0}', '{1}', " +
                $" NOW(), '{userId}', now(), '{bill.VenderId}', '{bill.PO}', '{bill.DefineBill}');";

            foreach (var item in bill.BillImportInfors)
            {
                //sql += $"INSERT INTO `STORE_MATERIAL_DB`.`BILL_INFOR` (`BILL_NUMBER`, `PART`, `QTY`) VALUES ('{item.BillNumber}', '{item.Part}', '{item.Qty}');";
            }
            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }

    }
}
