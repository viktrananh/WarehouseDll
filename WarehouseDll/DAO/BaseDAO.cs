using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO;
using WarehouseDll.DTO.FinishedProduct;

namespace WarehouseDll.DAO
{
    public class BaseDAO
    {
        public static SqlClass _MySql = new SqlClass("mariadbsv1.mvn", "TRACKING_SYSTEM", "admin", "ManuAdmin$123");
        public static SqlClass _SqlSever = new SqlClass("192.168.10.253\\PANACIM", "PANACIM", "sa", "PANASONIC1!");

        public bool IsListEmty<T>(List<T> myList)
        {
            if (myList != null && myList.Count != 0) return false;
            return true;
        }
        public bool IsTableEmty(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return true;
            return false;
        }

        public DateTime GetTimeServer()
        {
            return DateTime.Parse(_MySql.GetDataMySQL("SELECT NOW();").Rows[0][0].ToString());
        }
        public bool isPartMSD(string pathNumber, out string LvMSD)
        {
            LvMSD = string.Empty;
            DataTable ismsd = _MySql.GetDataMySQL($"SELECT * FROM STORE_MATERIAL_DB.MSD_PART where PART_NUMBER='{pathNumber}';");
            if (IsTableEmty(ismsd)) return false;
            LvMSD = "LEVEL" + _MySql.GetDataMySQL($"SELECT * FROM STORE_MATERIAL_DB.MSD_TYPE where MSD_INDEX = '{ismsd.Rows[0]["MSD_LEVEL"].ToString()}';").Rows[0]["MSD_TYPE"].ToString();
            return true;
        }
        public bool isPartPrintDoubleTem(string partNo)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM STORE_MATERIAL_DB.PRINT_2_TEM_PART WHERE PART_NUMBER = '{partNo}';");
            if (IsTableEmty(dt)) return false;
            return true;
        }
        public bool isPartCheckExprity(string interPart)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM STORE_MATERIAL_DB.MASTER_PART where PART_NUMBER = '{interPart}' AND ( MASTER_TYPE = 4 or MASTER_TYPE = 3 OR CHECK_EXP=1) ;");
            if (IsTableEmty(dt)) return false;
            return true;
        }
     
        public string equipmentESD(string did)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT IS_ESD FROM STORE_MATERIAL_DB.DATA_INPUT WHERE DID = '{did}';");
            if (IsTableEmty(dt)) return "";
            if (string.IsNullOrEmpty(dt.Rows[0][0].ToString())) return "";
            string esd = (dt.Rows[0][0].ToString() == "1") ? "ESD" : "";
            return esd;
        }
        public int getRePrint(string did)
        {
            int reprint = 0;
            DataTable dt = _MySql.GetDataMySQL($"SELECT RE_PRINT FROM STORE_MATERIAL_DB.DATA_INPUT WHERE DID='{did}';");
            if (IsTableEmty(dt)) return 0;
            if (!int.TryParse(dt.Rows[0][0].ToString(), out reprint)) return 0;
            return reprint;
        }
        public bool timeExpDID(string did, out DateTime date_exp)
        {
            date_exp = DateTime.MinValue;
            DataTable dt = _MySql.GetDataMySQL($"SELECT DATE_EXP FROM STORE_MATERIAL_DB.DATA_INPUT WHERE DID='{did}';");
            if (IsTableEmty(dt)) return false;
            date_exp = DateTime.Parse(dt.Rows[0]["DATE_EXP"].ToString());
            return true;
        }
        public bool isQrCodeOP(string pwd, out string userID)
        {
            userID = string.Empty;
            string sql = $"SELECT * FROM TRACKING_SYSTEM.USER_CONTROL where USER_PASSWORD = '{pwd}';";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return false;
            userID = dt.Rows[0]["USER_NAME"].ToString();
            return true;
        }
        public bool isMSDPart(string part, ref string MSDlevel)
        {
            MSDlevel = "";
            DataTable dt = _MySql.GetDataMySQL($"SELECT MSD_TYPE FROM STORE_MATERIAL_DB.MSD_TYPE inner join STORE_MATERIAL_DB.MSD_PART " +
                $"ON STORE_MATERIAL_DB.MSD_TYPE.MSD_INDEX = STORE_MATERIAL_DB.MSD_PART.MSD_LEVEL AND STORE_MATERIAL_DB.MSD_PART.PART_NUMBER = '{part}'; ");
            if (IsTableEmty(dt)) return false;
            MSDlevel = dt.Rows[0][0].ToString();
            return true;
        }
        public DataTable createManuTem(int number, out int year, out int week)
        {
            DataTable dtSeral = new DataTable();
            dtSeral.Columns.Add("Serial");

            int lasteTeimdata = 0;

            DateTime timeNow = GetTimeServer();
            year = timeNow.Year;
            week = GetIso8601WeekOfYear(timeNow);
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.MVN_TEM_DATA WHERE WEEK_CREATE = '{week}' AND YEAR_CREATE = '{year}' ORDER BY ID DESC;");
            if (!IsTableEmty(dt))
                lasteTeimdata = int.Parse(dt.Rows[0]["TEM_END"].ToString().Substring(5));
            for (int i = 1; i <= number; i++)
            {
                int temNew = lasteTeimdata + i;
                string tem = $"P{year.ToString().Substring(2)}{week}{temNew.ToString().PadLeft(5, '0')}";
                dtSeral.Rows.Add(tem);
            }
            return dtSeral;
        }
        public int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public BoxInfor GetBoxInfor(string boxSerial)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.BOX_LIST where BOX_SERIAL = '{boxSerial}';");

            if (IsTableEmty(dt)) return new BoxInfor();
            BoxInfor box = new BoxInfor(dt.Rows[0]);
            return box;
        }
        public List<FPBillType> GetFPBillTypes()
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_BILL_TYPE;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPBillType>();

            List<FPBillType> ls = (from r in dt.AsEnumerable()
                                   select new FPBillType()
                                   {
                                       Id = r.Field<int>("ID"),
                                       Name = r.Field<string>("NAME"),
                                       ActionId = r.Field<int>("ACTION_ID")
                                   }).ToList();
            return ls;
        }
        public List<BoxInfor> GetBoxChilden(string boxSerial)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.BOX_ID_GROUP A INNER JOIN TRACKING_SYSTEM.BOX_LIST B ON A.BOX_CHILD = B.BOX_SERIAL where BOX_PARENT = '{boxSerial}';");

            if (IsTableEmty(dt)) return new List<BoxInfor>();
            List<BoxInfor> boxInfors = new List<BoxInfor>();
            foreach (DataRow item in dt.Rows)
            {
                BoxInfor box = new BoxInfor(item);
                boxInfors.Add(box);
            }


            return boxInfors;
        }
        public int GetRemainWork(string work)
        {
            string sql = $"SELECT REMAIN FROM TRACKING_SYSTEM.FP_WORK_REMAIN where WORK_ID='{work}';";
            DataTable DT = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(DT)) return 0;
            if (string.IsNullOrEmpty(DT.Rows[0]["REMAIN"].ToString())) return 0;
            return int.Parse(DT.Rows[0]["REMAIN"].ToString());
        }
        public int GetRemainSFB(string work, string process)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT QTY FROM TRACKING_SYSTEM.SFP_REMAIN_CURRENT WHERE WORK_NUMBER= '{work}' AND LOCATION ='{process}' ;");
            if (IsTableEmty(dt)) return 0;
            if (string.IsNullOrEmpty(dt.Rows[0][0].ToString())) return 0;
            return int.Parse(dt.Rows[0][0].ToString());
        }

        public List<FPBill> GetAllFBBill(int billType, DateTime dateStart, DateTime dateEnd, bool noteDetail = true)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_BILLS WHERE TYPE_BILL = '{billType}' AND `TIME` >= '{dateStart.ToString("yyyy-MM-dd")}' AND `TIME` <= '{dateEnd.ToString("yyyy-MM-dd")}' AND STATE <> -1  ORDER BY ID DESC ; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPBill>();

            List<FPBill> lsBill = new List<FPBill>();
            foreach (DataRow item in dt.Rows)
            {
                FPBill bill = new FPBill(item, noteDetail);
                lsBill.Add(bill);
            }
            return lsBill;
        }
        public bool IsBoxFinish(string box)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.BOX_LIST A INNER JOIN TRACKING_SYSTEM.BOX_PACKING B ON A.BOX_LEVEL = B.`LEVEL` AND A.MODEL = B.MODEL_ID where A.BOX_SERIAL='{box}' AND B.IS_FINISH = 1; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return false;
            return true;
        }

        public List<FPBillDetail> GetBillDetailByBillId(string bill, bool notDetail = true)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_BILL_DETAILS A " +
                $" INNER JOIN TRACKING_SYSTEM.WORK_ORDER B ON A.WORK_ID = B.WORK_ID " +
                $"INNER JOIN TRACKING_SYSTEM.PART_MODEL_CONTROL C ON B.MODEL = C.ID_MODEL where bill_number='{bill}' ;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPBillDetail>();
            List<FPBillDetail> lsBill = new List<FPBillDetail>();
            foreach (DataRow item in dt.Rows)
            {
                FPBillDetail fPBillDetail = new FPBillDetail(item, notDetail);
                lsBill.Add(fPBillDetail);
            }
            return lsBill;
        }
        public bool IsWorkSEFLinkModel(string workId)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.MODEL_LINK A INNER JOIN  TRACKING_SYSTEM.WORK_ORDER B  ON A.MODEL_NAME COLLATE UTF8_UNICODE_CI = B.MODEL AND A.IS_REWORK = B.IS_RMA " +
                $"  where B.WORK_ID = '{workId}';";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return false;
            return true;
        }
        public FPBill GetFBBillByBillId(string billId)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.FP_BILLS WHERE BILL_NUMBER = '{billId}' ; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new FPBill();
            FPBill fPBill = new FPBill(dt.Rows[0], notDetail: false);
            return fPBill;
        }

        public List<FPBoxHistory> GetFPBoxHistory(string billNumber, string workId, bool IsModelLink)
        {
            string sql = $"select * FROM TRACKING_SYSTEM.FB_BOX_HISTORY A " +
                $" INNER JOIN TRACKING_SYSTEM.BOX_LIST B ON A.BOX_SERIAL = B.BOX_SERIAL WHERE BILL_NUMBER = '{billNumber}' AND WORK_ID = '{workId}' ;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPBoxHistory>();
            var ls = (from r in dt.AsEnumerable()
                      select new FPBoxHistory()
                      {
                          Id = r.Field<int>("ID"),
                          BillNumber = r.Field<string>("BILL_NUMBER"),
                          WorkId = r.Field<string>("WORK_ID"),
                          ModelId = r.Field<string>("MODEL_ID"),
                          CusId = r.Field<string>("CUS_ID"),
                          BoxSerial = r.Field<string>("BOX_SERIAL"),
                          QtyBefore = r.Field<int>("QTY_BEFORE"),
                          Qty = r.Field<int>("QTY"),
                          QtyAfter = r.Field<int>("QTY_AFTER"),
                          DatePacking = r.Field<DateTime>("DATE_PACKING"),
                          EventTime = r.Field<DateTime>("EVENT_TIME"),
                          BoxDetailWorks = !IsModelLink ? new List<BoxDetailWork>() : GetBoxDetailWorks(r.Field<string>("BOX_SERIAL"))
                      }).ToList();
            return ls;
        }

        public  List<BoxDetailWork> GetBoxDetailWorks(string box)
        {
            string sql = $"SELECT * FROM TRACKING_SYSTEM.BOX_DETAIL_WORK A INNER JOIN TRACKING_SYSTEM.WORK_ORDER B ON A.WORK_ID = B.WORK_ID where BOX_SERIAL='{box}';";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<BoxDetailWork>();
            List<BoxDetailWork> ls = (from r in dt.AsEnumerable()
                                      select new BoxDetailWork()
                                      {
                                          Id =r.Field<int>("ID"),
                                          BoxSerial = r.Field<string>("BOX_SERIAL"),
                                          WorkId = r.Field<string>("WORK_ID"),
                                          PO = r.Field<string>("PO"),
                                          ModelId = r.Field<string>("MODEL"),
                                          Qty = r.Field<int>("QTY")
                                      }).ToList();
            return ls;
        }
        public List<FPLocation> GetListBoxRemain(string bill, string work, int IsBook)
        {
            string sql = IsBook == 1 ? $"SELECT B.ID, B.WORK_ID, B.BOX_SERIAL, B.QTY, B.LOCATION, B.OP, B.BILL_NUMBER , B.CREAT_TIME  FROM TRACKING_SYSTEM.FP_BOOK_BILL_DETAIL A INNER JOIN TRACKING_SYSTEM.FP_LOCATION B" +
                $" ON A.BOX_SERIAL = B.BOX_SERIAL WHERE A.BILL_NUMBER = '{bill}' AND A.WORK_ID='{work}' AND A.STATE = 0;"
                 : $"SELECT * FROM TRACKING_SYSTEM.FP_LOCATION WHERE WORK_ID = '{work}' ;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<FPLocation>();

            List<FPLocation> ls = new List<FPLocation>();
            foreach (DataRow item in dt.Rows)
            {
                FPLocation lct = new FPLocation(item);
                ls.Add(lct);
            }
            return ls;
        }

        public string GetProcessStationPaking(string model, int IsRMA)
        {

            return IsRMA == 0 ? _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.DEFINE_STATION_ID WHERE MODEL_ID='{model}' AND STATION_NAME = 'PACKING';").Rows[0]["PROCESS"].ToString()
                : _MySql.GetDataMySQL($"SELECT * FROM RMA_SYSTEM.DEFINE_STATION_ID WHERE MODEL_ID='{model}' AND STATION_NAME = 'PACKING';").Rows[0]["PROCESS"].ToString();

        }

        public bool FPBillExist(string bill)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.FP_BILLS where BILL_NUMBER = '{bill}';");
            if (IsTableEmty(dt)) return false;
            return true;
        }
    }
}
