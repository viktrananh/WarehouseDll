using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.BUS.Material
{
    public class BillExportMaterialBUS : BaseBUS
    {
        public bool CreatDataInput(List<DataInput>  dataInputs, Bill bill, string userId, string reprint)
        {
            string sql = string.Empty;
            foreach (var item in dataInputs)
            {
               
                sql += $"INSERT INTO `STORE_MATERIAL_DB`.`DATA_INPUT` (`VENDOR`,`DID`, `PART_NUMBER`, `WORK_ID`,`MODEL`,`BUYER`,`DEFINE_EXPORT`, `QTY`, " +
                    $" `CUSTOMER`,  `OPERATOR`, `TIME_INPUT`,`CUST_KP_NO`,`MFR_KP_NO`, " +
                    $" `MFR_CODE`,`DATE_CODE`,`LOT_CODE`,`MACHINE`, " +
                    $"`DATE_EXP`,  `DATE_EXP_REAL` , " +
                    $" `BILL_NUMBER`,`IS_ESD`, `RE_PRINT`) VALUES " +
                    $"('{item.VenderName}','{item.DID}', '{item.PartNumber}', '{bill.WorkId}','{bill.ModelId}','{item.VenderName}','{item.DefineExport}', '{item.Qty}'," +
                    $" '{bill.CusId}',  '{userId}', now(),'{item.CusPart}','{item.MFGPart}'," +
                    $"'{item.VenderPart}','{item.DateCode}','{item.LotCode}','{item.Machine}'," +
                    $" '{item.DateEXP}', '{item.DateEXPReal}' " +
                    $"'{bill.BillNumber}', '{item.IsESD}',  '{reprint}');";
            }

            if (_MySql.InsertDataMySQL(sql)) return true;
            return false;
        }
    }
}
