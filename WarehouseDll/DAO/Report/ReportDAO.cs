using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.FinishedProduct;

namespace WarehouseDll.DAO.Report
{
    public class ReportDAO : BaseDAO
    {
        public List< FPWorkRemain> GetALlWorkRemain()
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_WORK_REMAIN where REMAIN > 0;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPWorkRemain>();
            List<FPWorkRemain> ls = (from r in dt.AsEnumerable()
                                     select new FPWorkRemain()
                                     {
                                         Id = r.Field<int>("ID"),
                                         WorkId = r.Field<string>("WORK_ID"),
                                         Remain = r.Field<int>("REMAIN"),
                                         Imp = r.Field<int>("IMP"),
                                         Exp = r.Field<int>("EXP"),
                                         ImpLocal = r.Field<int>("IMP_LOCAL"),
                                         ImpCus = r.Field<int>("IMP_CUS"),
                                         ExpCus = r.Field<int>("EXP_CUS"),
                                         ExpLocal = r.Field<int>("EXP_LOCAL"),
                                     }).ToList();
            return ls;
        }
    }
}
