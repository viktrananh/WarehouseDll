using MySqlX.XDevAPI;
using ROSE_Dll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.FinishedProduct;
using Location = WarehouseDll.DTO.FinishedProduct.Location;

namespace WarehouseDll.DAO.FinishedProduct
{
    public class FPBillImportDAO : BaseDAO
    {
        public string CreateBillImportFinishProduct(string area, int typeBill)
        {
            string bill = string.Empty;

            string clause = string.Empty;
            DateTime timeNow = GetTimeServer();
            string sql = string.Empty;
            clause = timeNow.ToString("ddMMyyyyHHmmss");
            bill = clause + $"/{area}-TP";
            return bill;
        }
 

    }
}
