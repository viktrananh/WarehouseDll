using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseDll.DTO.Material;

namespace WarehouseDll.DAO.Material
{
    public class BaseMaterialBillDAO : BaseDAO
    {

        public List<Bill> GetLsMaterialBill(int typeBill, DateTime start, DateTime end, int stateId = -1)
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

        public List<BillInfor> GetBillInfors(string billNumber)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.BILL_INFOR where BILL_NUMBER = '{billNumber}';";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<BillInfor>();

            List<BillInfor> ls = new List<BillInfor>();
            foreach (DataRow item in dt.Rows)
            {
                BillInfor billInfor = new BillInfor(item);
                ls.Add(billInfor);
            }
            return new List<BillInfor>();

        }


        public Bill GetBillByBillId(string billId)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.BILL where BILL_NUMBER = '{billId}';";
            DataTable dt = _MySql.GetDataMySQL(sql);

            if (IsTableEmty(dt)) return new Bill();

            Bill bill = new Bill(dt.Rows[0]);

          
            return bill;
        }

    }
}
