using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO.Material;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.DAO.Material
{
    public class BillImportDAO : BaseDAO
    {
        ROSE_Dll.DAO.BomDao BomDao = new ROSE_Dll.DAO.BomDao();
        public BillImport GetBillImport(string billID)
        {
            string cmd = $@"SELECT * FROM STORE_MATERIAL_DB.BILL_REQUEST_IMPORT where BILL_NUMBER='{billID}'; ";
            DataTable dt = _MySql.GetDataMySQL(cmd);
            BillImport billImport = new BillImport();
            if (IsTableEmty(dt)) return new BillImport();
            billImport.BillNumber = billID;
            billImport.VenderId = dt.Rows[0]["VENDER_ID"].ToString();
            billImport.CreatTime = DateTime.Parse(dt.Rows[0]["CREATE_TIME"].ToString());
            billImport.StateId = int.Parse(dt.Rows[0]["STATUS_BILL"].ToString());
            billImport.TypeBill = int.Parse(dt.Rows[0]["TYPE_BILL"].ToString());
            billImport.CusId = dt.Rows[0]["CUS_ID"].ToString();
            billImport.ModelId = dt.Rows[0]["MODEL_ID"].ToString();
            billImport.WorkId = dt.Rows[0]["WORK_ID"].ToString();
            billImport.PartsInfor = BomDao.GetManuMasterParts(billImport.VenderId);
            if (billImport.VenderId == "CEV")
            {
                for (int i = 0; i < billImport.PartsInfor.Count(); i++)
                {
                    if (billImport.PartsInfor[i].mfgPart.Contains("$") || billImport.PartsInfor[i].mfgPart.Contains("@"))
                    {
                        string[] list = billImport.PartsInfor[i].mfgPart.Split('$')[0].Split('@');
                        string bomMfg = string.Join("", list.Where(a => string.IsNullOrEmpty(a) == false).ToList());
                        billImport.PartsInfor[i].mfgPart = bomMfg;

                    }
                }
            }
            billImport.IsImportCusPart = isUseTemCus(billImport.VenderId);
            billImport.PartSpecialEXPs = GetPartSpecialEXPs(billID);
            billImport.BillImportInfors = GetListPart(billID);
            //billImport.Def = int.Parse(dt.Rows[0]["define_export"].ToString());
            billImport.Note = dt.Rows[0]["note"].ToString();
            return billImport;
        }

     
        public bool isUseTemCus(string cusID)
        {
            DataTable dt = _MySql.GetDataMySQL($"SELECT * FROM TRACKING_SYSTEM.DEFINE_CUSTOMER where CUSTOMER_ID='{cusID}' and CHECK_CUSPART=1;");
            if (IsTableEmty(dt))
                return false;
            return true;
        }
        public List<PartSpecialEXP> GetPartSpecialEXPs(string billID)
        {
            string cmd = $"SELECT PART_NUMBER,DATE_EXP,TYPE_SPECIAL FROM STORE_MATERIAL_DB.IQC_SPECIAL_CONFIRM_EXP where BILL_NUMBER='{billID}';";
            DataTable dt = _MySql.GetDataMySQL(cmd);
            if (IsTableEmty(dt))
                return new List<PartSpecialEXP>();
            return (from a in dt.AsEnumerable()
                    select new PartSpecialEXP
                    {
                        TypeSpecial = a.Field<int>("TYPE_SPECIAL"),
                        PartNumber = a.Field<string>("PART_NUMBER"),
                        DateEXP = a.Field<int>("TYPE_SPECIAL") == 1 ? a.Field<DateTime>("DATE_EXP").ToString() : null

                    }).ToList();
        }
        public List<BillImport> GetLsBillImport(DateTime start , DateTime end, bool loadDetail = false)
        {
            string sql = $@"SELECT  A.BILL_NUMBER, A.CUSTOMER, A.WORK_ID, A.CREATE_BY, A.CREATE_TIME,A.VENDER_ID, C.VENDER_NAME,  B.`STATUS`,  A.STATUS_BILL FROM STORE_MATERIAL_DB.BILL_REQUEST_IMPORT A
                         INNER JOIN STORE_MATERIAL_DB.BILL_STATUS B  ON A.TYPE_BILL = B.TYPE_ID AND A.STATUS_BILL = B.ID
                         LEFT JOIN TRACKING_SYSTEM.DEFINE_VENDER C ON A.VENDER_ID = C.VENDER_ID where A.TYPE_BILL = 2  and CREATE_TIME >='{start.ToString("yyyy-MM-dd HH:mm:ss")}' and  CREATE_TIME <='{end.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY CREATE_TIME DESC; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new List<BillImport>();
            List<BillImport> ls = new List<BillImport>();
            foreach (DataRow item in dt.Rows)
            {
                BillImport bill = new BillImport(item, loadDetail);
                ls.Add(bill);
            }
            return ls;
        }

        public BillImport GetBillImportByBillId(string billId)
        {
            string sql = $@"SELECT  A.BILL_NUMBER, A.CUSTOMER, A.WORK_ID, A.CREATE_BY, A.CREATE_TIME, A.VENDER_ID, C.VENDER_NAME,  B.`STATUS`,  A.STATUS_BILL FROM STORE_MATERIAL_DB.BILL_REQUEST_IMPORT A
                         INNER JOIN STORE_MATERIAL_DB.BILL_STATUS B  ON A.TYPE_BILL = B.TYPE_ID AND A.STATUS_BILL = B.ID
                         LEFT JOIN TRACKING_SYSTEM.DEFINE_VENDER C ON A.VENDER_ID = C.VENDER_ID where A.BILL_NUMBER = '{billId}' ; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new BillImport();
            BillImport bill = new BillImport(dt.Rows[0], true);
            return bill;
        }
        public List<BillImportInfor> GetListPart(string billID)
        {
            string cmd = $"SELECT * FROM STORE_MATERIAL_DB.BILL_CUS_INPUT where BILL_NUMBER='{billID}';";
            DataTable dt = _MySql.GetDataMySQL(cmd);
            if (IsTableEmty(dt)) return new List<BillImportInfor>();
            var ls = (from r in dt.AsEnumerable()
                      select new BillImportInfor()
                      {
                          //BillNumber = r.Field<string>("BILL_NUMBER"),
                          //Part = r.Field<string>("CUS_PART"),
                          //req = r.Field<int>("QTY"),
                          //ReciveQty = r.Field<int>("RECIVE_QTY")
                      }).ToList();
            return ls;
        }

        public List<BillImportPO> GetBillImportPO(string billID)
        {
            string cmd = $"SELECT * FROM STORE_MATERIAL_DB.BILL_IMPORT_PO where BILL_NUMBER = '{billID}';";
            DataTable dt = _MySql.GetDataMySQL(cmd);
            if (IsTableEmty(dt)) return new List<BillImportPO>();
            var ls = (from r in dt.AsEnumerable()
                      select new BillImportPO()
                      {
                          BillNumber = r.Field<string>("BILL_NUMBER"),
                          PO = r.Field<string>("PO"),
                          CusModel = r.Field<string>("CUS_MODEL"),
                          ModelId = r.Field<string>("MODEL_ID"),
                          CreatTime = r.Field<DateTime>("CREAT_TIME")
                      }).ToList();
            return ls;
        }
        public bool isOPComFirm(string pass, out string OperName)
        {

            OperName = string.Empty;
            DataTable dtnhanvien = _SqlSever.GetDataSQL($" select * from [PanaCIM].[dbo].[operator] where OPERATOR_BARCODE='{pass.Substring(1)}' ;");
            if (IsTableEmty(dtnhanvien)) return false;
            OperName = dtnhanvien.Rows[0]["OPERATOR_NAME"].ToString().Trim();
            return true;
        }
        public bool getdetailDIDPanacim(string did, ref string work, ref string part, ref string qty, ref string cus, ref string model, ref string cusPart, ref string mfgpart, ref string lotcode, ref string datecode)
        {
            DataTable dt = _SqlSever.GetDataSQL($"  SELECT PART_NO,LOT_NO,CURRENT_QUANTITY,REEL_BARCODE,VENDOR_NO,USER_DATA, USER_DATA_2,USER_DATA_3,USER_DATA_4,USER_DATA_5 FROM [PanaCIM].[dbo].[reel_data] A INNER JOIN [PanaCIM].[dbo].[reel_user_data] B " +
                $"ON A.REEL_ID = B.REEL_ID  AND A.REEL_BARCODE = '{did}'; ");
            if (IsTableEmty(dt)) return false;
            if (int.Parse(dt.Rows[0]["CURRENT_QUANTITY"].ToString()) < 0) return false;
            work = dt.Rows[0]["USER_DATA_2"].ToString();
            part = dt.Rows[0]["PART_NO"].ToString();
            cus = dt.Rows[0]["USER_DATA"].ToString();
            model = dt.Rows[0]["USER_DATA_3"].ToString();
            qty = dt.Rows[0]["CURRENT_QUANTITY"].ToString();
            lotcode = dt.Rows[0]["LOT_NO"].ToString();
            datecode = dt.Rows[0]["LOT_NO"].ToString();
            mfgpart = dt.Rows[0]["USER_DATA_4"].ToString();
            cusPart = mfgpart;

            return true;
        }
        public void loadBillInputQty(BillImport billImport)
        {

            string cmd = "";
            switch (billImport.TypeBill)
            {
                case 2:
                    cmd = $"SELECT sum(QTY) as qty, sum(RECIVE_QTY) as qty_recive FROM STORE_MATERIAL_DB.BILL_IMPORT_CUS where BILL_NUMBER='{billImport.BillNumber}';";
                    break;
                case 3:
                    cmd = $"SELECT sum(qty) as qty,(select sum(qty) from STORE_MATERIAL_DB.BILL_EXPORT_PC where bill_number = '{billImport.BillNumber}' and status = 0)  'qty_recive'  " +
                        $" FROM STORE_MATERIAL_DB.BILL_EXPORT_PC where bill_number = '{billImport.BillNumber}'; ";
                    break;
                case 1:
                    cmd = $"SELECT sum(QTY) as qty, sum(RECIVE_QTY) as qty_recive FROM STORE_MATERIAL_DB.BILL_IMPORT_CUS where BILL_NUMBER='{billImport.BillNumber}';";
                    break;

            }
            DataTable dt = _MySql.GetDataMySQL(cmd);
            //billImport.qt = int.Parse(dt.Rows[0]["qty"].ToString());
            billImport.qty_recived = int.Parse(dt.Rows[0]["qty_recive"].ToString());
        }

        public DataTable loadBill_IPDetailByPart(BillImport billImport, bool showprice = false)
        {
            string cmd = "";
            string subcmd = showprice ? ",UNIT_PRICE `Giá` , VAT , total_cash `Tổng giá`" : "";

            if (billImport.TypeBill == billImport.PN_CCDC_TU_Khach)
            {
                cmd = $"SELECT BILL_IMPORT_CUS.Part_number,cus_part,mfg_part,description, QTY as `Số lượng`,RECIVE_QTY as `Thực nhận`{subcmd}, RECIVE_QTY-QTY as `Chênh lệch` FROM STORE_MATERIAL_DB.BILL_IMPORT_CUS inner join STORE_MATERIAL_DB.MASTER_PART" +
                      $" on STORE_MATERIAL_DB.BILL_IMPORT_CUS.part_number  collate utf8_general_ci=STORE_MATERIAL_DB.MASTER_PART.part_number where BILL_NUMBER='{billImport.BillNumber}';";
            }
            else if (billImport.TypeBill == billImport.PN_NVL_TU_Khach)
            {
                cmd = $"SELECT Part_number, CUS_PART as `Mã khách hàng`,mfg_part as `Mã nhà sản xuất`,QTY as `Số lượng`,RECIVE_QTY as `Thực nhận`{subcmd} , RECIVE_QTY-QTY as `Chênh lệch` FROM STORE_MATERIAL_DB.BILL_IMPORT_CUS" +
                  $" where BILL_NUMBER='{billImport.BillNumber}';";
            }
            else if (billImport.TypeBill == billImport.PN_NVL_TU_SX)
            {
                cmd = $"SELECT PART_NUMBER as `Mã nội bộ`,QTY as `Số lượng`,START_TIME as `thời gian bắt đầu`,LAST_TIME as `thời gian kết thúc` FROM STORE_MATERIAL_DB.BILL_WH_INPUT where BILL_NUMBER='{billImport.BillNumber}';";
            }
            DataTable dt = _MySql.GetDataMySQL(cmd);
            return dt;

        }


        public List<Bill> GetAllBill(int billType)
        {
            return new List<Bill>();
        }

        public Bill GetBillByBillId(int billId)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.BILL where BILL_NUMBER='{billId}'; ";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new Bill();
            Bill bill = new Bill(dt.Rows[0]);
            return bill;

        }

        public int GetDataMaxOrder(string partnumber)
        {
            string sql = $"SELECT MAX(DID) FROM STORE_MATERIAL_DB.DATA_INPUT WHERE PART_NUMBER='{partnumber}' and  TIME_INPUT = (SELECT MAX(TIME_INPUT) FROM STORE_MATERIAL_DB.DATA_INPUT WHERE PART_NUMBER='{partnumber}' AND MONTH(TIME_INPUT) = MONTH(NOW()) AND LENGTH(DID) = 24) AND LENGTH(DID) = 24;";
            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return 0;
            return Convert.ToInt32(dt.Rows[0][0].ToString().Split('_')[1].Substring(0, 4));

        }

        public Bill GetBillByBillId(string billId)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.BILL where BILL_NUMBER='{billId}'; ";

            DataTable dt = _MySql.GetDataMySQL(sql);
            if (IsTableEmty(dt)) return new Bill();
            Bill bill = new Bill(dt.Rows[0]);
            return bill;
        }
    }
}
