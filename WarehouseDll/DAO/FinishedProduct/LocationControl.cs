using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DAO;
using WarehouseDll.DTO.FinishedProduct;

namespace WarehouseDll.DAO.FinishedProduct
{
    public class LocationControl : BaseDAO
    {


        public List<Location> LoadListLocation()
        {
            List<Location> listLocation = new List<Location>();
            string sql = "SELECT A.LOCATION, A.WORK_ID, B.MODEL, B.BOX_SERIAL , B.COUNT, C.CUSTOMER_ID , A.CREAT_TIME , B.TIME_PACKING  FROM TRACKING_SYSTEM.FP_LOCATION A  " +
                " inner join TRACKING_SYSTEM.BOX_LIST B on A.BOX_SERIAL = B.BOX_SERIAL " +
                " INNER JOIN TRACKING_SYSTEM.PART_MODEL_CONTROL C ON B.MODEL COLLATE UTF8_UNICODE_CI = C.ID_MODEL  ORDER BY A.LOCATION ;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            listLocation = (from r in dt.AsEnumerable()
                            group r by new
                            {
                                Cus = r.Field<string>("CUSTOMER_ID"),
                                Location = r.Field<string>("LOCATION"),
                                Work = r.Field<string>("WORK_ID"),
                                Model = r.Field<string>("MODEL")
                            } into gr
                            let lct = gr.Key.Location
                            let boxs = gr.Select(x => new BoxReportLocation()
                            {
                                BoxSerial = x.Field<string>("BOX_SERIAL"),
                                Qty = x.Field<int>("COUNT"),
                                Location = lct,
                                TimeImport = x.Field<DateTime>("CREAT_TIME"),
                                TimePacking = x.Field<DateTime>("TIME_PACKING"),
                                BoxInfor = new FPBillExportDAO().GetBoxInfor(x.Field<string>("BOX_SERIAL"))
                            }).ToList()
                            select new Location()
                            {
                                LocationName = lct,
                                WorkId = gr.Key.Work,
                                ModelId = gr.Key.Model,
                                Boxs = boxs,
                                BoxNumber = boxs.Count(),
                                PcbNumber = gr.Sum(x => x.Field<int>("COUNT")),
                                CusID = gr.Key.Cus,

                            }).ToList();

            return listLocation;
        }
    }
}
