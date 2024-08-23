using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO;

namespace WarehouseDll.DTO
{
    public class BoxInfor
    {
        public string BoxSerial { get; set; }
        public string Model { get; set; }
        public string WorkId { get; set; }
        public string LotNo { get; set; }
        public int State { get; set; }
        public int IsFQC { get; set; }
        public string LotFQC { get; set; }
        public int Level { get; set; }
        public int Qty { get; set; }
        public string Location { get; set; }
        public DateTime DatePacking { get; set; }
        public DateTime TimePacking { get; set; }
        public int IsBlock { get; set; }
        public int BoxLevel { get; set; }
        public List<BoxInfor> BoxChilden { get; set; }
        public BoxInfor() { }

        public BoxInfor(DataRow row)
        {
            BoxSerial = row["BOX_SERIAL"].ToString();
            Model = row["MODEL"].ToString();
            WorkId = row["WORK_ORDER"].ToString();
            LotNo = row["LOTNO"].ToString();
            State = int.Parse(row["STATE"].ToString());
            IsFQC = int.Parse(row["FQC"].ToString());
            LotFQC = row["LOT_FQC"].ToString();
            Level = int.Parse(row["BOX_LEVEL"].ToString());
            Qty = int.Parse(row["COUNT"].ToString());
            DatePacking = DateTime.Parse(row["DATE_PACKING"].ToString());
            TimePacking = DateTime.Parse(row["TIME_PACKING"].ToString());
            BoxLevel = int.Parse(row["BOX_LEVEL"].ToString());
            IsBlock = int.Parse(row["IS_BLOCK"].ToString());
            BoxChilden = Level != 1 ? new BaseDAO().GetBoxChilden(BoxSerial) : new List<BoxInfor>();
        }
    }
}
