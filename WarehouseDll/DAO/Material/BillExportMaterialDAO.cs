using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDll.DTO;
using WarehouseDll.DTO.Material.Export;
using WarehouseDll.DTO.Material.Import;

namespace WarehouseDll.DAO.Material
{
    public class BillExportMaterialDAO : BaseDAO
    {

        public List<BillExportMaterial> GetListMaterial(string workId = "")
        {
            return new List<BillExportMaterial>();
        }

        List<BillExportMaterialDetailReport> LoadInforExportMaterialByWork(string work, string modelId, List<BillExportMaterialDetail> _BillExportMaterialDetailSample)
        {

           var  _BillExportMaterialDetailReportS = new List<BillExportMaterialDetailReport>();
            var ls = GetListSummaryBillExportRequestPC(work, false);
            _BillExportMaterialDetailReportS = (from r in _BillExportMaterialDetailSample
                                                select new BillExportMaterialDetailReport()
                                                {
                                                    MainPart = r.MainPart,
                                                    PartNumber = r.PartNumber,
                                                }).ToList();
            foreach (var item in ls)
            {

                string bill = item.BillId;
                int type = item.BillType; // loại phiếu

                var danhsachPartDaXuatTrongPhieu = getBill_Exported(bill);
                foreach (var b in item.BillExportMaterialDetails)
                {
                    string main = b.MainPart;
                    string sub = b.PartNumber;
                    float qtyRequest = b.Qty;
                    if (string.IsNullOrEmpty(main))
                    {
                        continue;
                    }

                    if (!main.Contains(modelId)) continue;
                    if (type == LoadSubTypeBillExportMaterial.BOM || type == LoadSubTypeBillExportMaterial.FILE || type == LoadSubTypeBillExportMaterial.FORMING)
                    {
                        _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).Qty += qtyRequest;
                    }
                    else
                    {
                        _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).QtyArise += qtyRequest;
                    }

                    string partNumber = b.PartNumber;
                    if (partNumber.Contains("\n") || partNumber.Contains(" "))
                    {
                        string[] partArr = partNumber.Split('\n');
                        if (partArr.Length < 2)
                            partArr = partNumber.Split(' ');
                        partNumber = partNumber.Replace("\n", "  ");
                        for (int i = 0; i < partArr.Length; i++)
                        {
                            int exportInt = danhsachPartDaXuatTrongPhieu.Find(a => a.partNo == partArr[i]).qty;
                            if (type == LoadSubTypeBillExportMaterial.BOM || type == LoadSubTypeBillExportMaterial.FILE || type == LoadSubTypeBillExportMaterial.FORMING)
                            {
                                _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).RealExport += exportInt;
                            }
                            else
                            {
                                _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).RealExportArise += exportInt;

                            }
                        }
                    }
                    else
                    {
                        int exportInt = danhsachPartDaXuatTrongPhieu.Find(a => a.partNo == partNumber).qty;
                        if (type == LoadSubTypeBillExportMaterial.BOM || type == LoadSubTypeBillExportMaterial.FILE || type == LoadSubTypeBillExportMaterial.FORMING)
                        {
                            _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).RealExport += exportInt;
                        }
                        else
                        {
                            _BillExportMaterialDetailReportS.FirstOrDefault(x => x.MainPart == main).RealExportArise += exportInt;

                        }
                    }
                }
            }
            return _BillExportMaterialDetailReportS;
        }
        public List<BillExportMaterial> GetListSummaryBillExportRequestPC(string work = "", bool NotDetail = true)
        {
            BillExportMaterial Bill_Export = new BillExportMaterial();

            string condition = string.IsNullOrEmpty(work) ? "" : $" and WORK_ID like '%{work}%'";
            DataTable dt = _MySql.GetDataMySQL($"SELECT  * FROM STORE_MATERIAL_DB.BILL_REQUEST_EX where STATUS_EXPORT <>-1 AND month( TIME_CREATE) > month(now()) -2  AND " +
                $" ( TYPE_BILL = {BillExportMaterial.BillPTH} OR TYPE_BILL= {BillExportMaterial.BillSMT}) {condition} ORDER BY TIME_CREATE DESC ;");
            if (IsTableEmty(dt)) return new List<BillExportMaterial>();

            List<BillExportMaterial> billExxports = new List<BillExportMaterial>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var subType = int.Parse(dt.Rows[i]["SUB_TYPE"].ToString());
                string loaiphieu = LoadSubTypeBillExportMaterial.SubTypeBillExportMaterial.FirstOrDefault(x => x.Id == subType).Name;
                int statusid = int.Parse(dt.Rows[i]["STATUS_EXPORT"].ToString());
                string trangthai = LoadStatusBillExportMaterial.StatusBillExportMaterial.Where(a => a.Id == int.Parse(dt.Rows[i]["STATUS_EXPORT"].ToString())).FirstOrDefault().Name;

                BillExportMaterial bill = new BillExportMaterial()
                {
                    BillId = dt.Rows[i]["BILL_NUMBER"].ToString(),
                    WorkId = dt.Rows[i]["WORK_ID"].ToString(),
                    BillType = int.Parse(dt.Rows[i]["SUB_TYPE"].ToString()),
                    BillTypeName = loaiphieu,
                    BillStatus = int.Parse(dt.Rows[i]["STATUS_EXPORT"].ToString()),
                    BillStatusName = trangthai,
                    OP = dt.Rows[i]["OP"].ToString(),
                    BomVersion = dt.Rows[i]["BOM_VERSION"].ToString(),
                    CreateTime = DateTime.Parse(dt.Rows[i]["TIME_CREATE"].ToString()),
                    BillExportMaterialDetails = NotDetail ? new List<BillExportMaterialDetail>() : LoadBillDetail(dt.Rows[i]["BILL_NUMBER"].ToString()),
                };
                billExxports.Add(bill);
                //rs.Rows.Add(dt.Rows[i]["WORK_ID"].ToString(), dt.Rows[i]["BILL_NUMBER"].ToString(), loaiphieu, trangthai, dt.Rows[i]["OP"].ToString(), dt.Rows[i]["TIME_CREATE"].ToString());
            }
            return billExxports;
        }

        public List<BillExportMaterialDetail> LoadBillDetail(string bill)
        {
            string sql = $"SELECT * FROM STORE_MATERIAL_DB.REQUEST_EXPORT  WHERE BILL_NUMBER = '{bill}';";
            var danhsachPartDaXuatTrongPhieu = getBill_Exported(bill);

            DataTable dt = _MySql.GetDataMySQL(sql);

            if (IsTableEmty(dt)) return new List<BillExportMaterialDetail>();
            List<BillExportMaterialDetail> ls = new List<BillExportMaterialDetail>();
            foreach (DataRow item in dt.Rows)
            {
                string main = item["MAIN_PART"].ToString();
                string sub = item["PART_NUMBER"].ToString();
                float qtyRequest = float.Parse(item["QTY"].ToString());
                if (string.IsNullOrEmpty(main))
                {
                    continue;
                }
                BillExportMaterialDetail billExportMaterialDetail = new BillExportMaterialDetail();
                billExportMaterialDetail.MainPart = main;
                billExportMaterialDetail.PartNumber = sub;
                billExportMaterialDetail.Qty += qtyRequest;
                string partNumber = sub;
                if (partNumber.Contains("\n") || partNumber.Contains(" "))
                {
                    string[] partArr = partNumber.Split('\n');
                    if (partArr.Length < 2)
                        partArr = partNumber.Split(' ');
                    partNumber = partNumber.Replace("\n", "  ");
                    for (int i = 0; i < partArr.Length; i++)
                    {
                        int exportInt = danhsachPartDaXuatTrongPhieu.Find(a => a.partNo == partArr[i]).qty;
                        billExportMaterialDetail.RealExport += exportInt;
                    }
                }
                else
                {
                    int exportInt = danhsachPartDaXuatTrongPhieu.Find(a => a.partNo == partNumber).qty;
                    billExportMaterialDetail.RealExport += exportInt;
                }
                ls.Add(billExportMaterialDetail);
            }

            return ls;
        }
        public List<partXuat> getBill_Exported(string billExport)
        {
            List<partXuat> parts = new List<partXuat>();
            string cmd = $"SELECT PART_NUMBER, SUM(QTY) FROM STORE_MATERIAL_DB.BILL_EXPORT_WH  WHERE BILL_REQUEST='{billExport}'  GROUP BY PART_NUMBER ;";
            DataTable dt = _MySql.GetDataMySQL(cmd);
            if (IsTableEmty(dt)) return parts;
            foreach (DataRow item in dt.Rows)
            {
                parts.Add(new partXuat { partNo = item[0].ToString(), qty = int.Parse(item[1].ToString()) });
            }
            return parts;
        }
        public struct partXuat
        {
            public string partNo;
            public int qty;
        }
        public int GetRemainPanacim(string partNumber)
        {
            string sql = $"SELECT PART_NUMBER,SUM(ESTIMATED_QUANTITY) as REMAIN  FROM PanaCIM.dbo.unloaded_reel_view where  AREA='KHOSMT'  and PART_NUMBER='{partNumber}'  group by PART_NUMBER;";

            DataTable dt =_SqlSever.GetDataSQL(sql);
            if (IsTableEmty(dt)) return 0;
            return int.Parse(dt.Rows[0]["REMAIN"].ToString());
        }

        //public List<partXuat> getRemainKhoSMTPartsbyBillRequest(BillExport bill)
        //{
        //    List<partXuat> danhsachPartTrongPhieuTonkho = new DAO_Export().GetBill_request_new(bill);

        //    foreach (var item in danhsachPartTrongPhieuTonkho)
        //    {
        //        string[] parts = item.partNo.Split('\n');
        //        if (parts.Length < 2)
        //            parts = item.partNo.Split(' ');
        //        foreach (var a in parts)
        //        {
        //            if (string.IsNullOrEmpty(a))
        //                continue;
        //            if (danhsachPartTrongPhieuTonkho.Exists(b => b.partNo == a))
        //                continue;
        //            danhsachPartTrongPhieuTonkho.Add(new partXuat { partNo = a.Trim(), qty = GetRemainPanacim(a) });
        //        }
        //    }
        //    return danhsachPartTrongPhieuTonkho;
        //}
    }
}
