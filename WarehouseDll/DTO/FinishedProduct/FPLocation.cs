using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class FPLocation
    {
        public int Id { get; set; }
        public string BoxSerial { get; set; }
        public string WorkId { get; set; }
        public string Location { get; set; }
        public string OP { get; set; }
        public DateTime CreatTime { get; set; }
        public int Qty { get; set; }
        public string BillNumber { get; set; }
        public BoxInfor BoxInfor { get; set; }

        public FPLocation() { }
        public FPLocation(DataRow row)
        {
            Id = int.Parse( row["ID"].ToString());
            WorkId = row["WORK_ID"].ToString();
            BoxSerial = row["BOX_SERIAL"].ToString();
            Location = row["LOCATION"].ToString();
            CreatTime = DateTime.Parse( row["CREAT_TIME"].ToString());
            OP = row["OP"].ToString();
            Qty = int.Parse( row["QTY"].ToString());
            BillNumber = row["BILL_NUMBER"].ToString();
            BoxInfor = new BaseDAO().GetBoxInfor(row["BOX_SERIAL"].ToString());
        }

    }
}
