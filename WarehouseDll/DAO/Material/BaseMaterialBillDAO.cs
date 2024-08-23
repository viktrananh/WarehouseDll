using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.DAO.Material
{
    public class BaseMaterialBillDAO : BaseDAO
    {

        public List<Bill> GetLsMaterialBill(int typeBill, DateTime start, DateTime end)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.BILL where TYPE_BILL = '{typeBill}' AND CREAT_TIME >= '{start}' AND CREAT_TIME <= '{end}';";
            DataTable dt = _MySql.GetDataMySQL(sql);

            List<Bill> ls = new List<Bill>  ();

            foreach (DataRow item in dt.Rows)
            {
                Bill bill = new Bill(item);
                ls.Add(bill);
            }
            return ls;
        }
    }
}
