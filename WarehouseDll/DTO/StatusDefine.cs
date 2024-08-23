using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO
{
    public class StatusDefine
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public static class LoadStatusDefineCustomer
    {
        public const int DELETE = -1;
        public const int CREATE = 0;
        public const int UPDATE = 1;
        public const int LOCK = 2;
        public const int LOAD_LIST_MODEL = 3;
        public const int CREATE_NEW_MODEL = 4;


        public static List<StatusDefine> statusDefineCustomers = new List<StatusDefine>();
        static LoadStatusDefineCustomer()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = DELETE,
                Name = "Xóa khách hàng"
            });
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới khách hàng"
            });
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = UPDATE,
                Name = "Cập nhật thông tin khách hàng"
            });
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = LOCK,
                Name = "Khóa"
            });
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = LOAD_LIST_MODEL,
                Name = "Danh sách sản phẩm"
            });
            statusDefineCustomers.Add(new StatusDefine()
            {
                Id = CREATE_NEW_MODEL,
                Name = "Tạo sản phẩm mới"
            });
        }

    }
    /// <summary>
    /// Phiếu nhập linh kiện
    /// </summary>
    public static class LoadFunctionBillImportMaterial
    {
        public const int CANCEL = -1;
        public const int CREATE = 0;
        public const int UPDATE = 1;

        public static List<StatusDefine> statusBillImportMaterial = new List<StatusDefine>();
        static LoadFunctionBillImportMaterial()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = CANCEL,
                Name = "Xóa Sản phẩm"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới phiếu"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = UPDATE,
                Name = "Cập nhật thông tin Sản phẩm"
            });

        }

    }
    public static class LoadStatusBillImportMaterial
    {
        public const int KHOI_TAO = 0;
        public const int XAC_NHAN_CHO_NHAP = 1;
        public const int KHAI_BAO_DU_LIEU = 2;
        public const int NHAP_KHO = 3;
        public const int XAC_NHAN_NHAP_LOI = 4;
        public const int XAC_NHAN_THU_KHO = 5;
        public const int XAC_NHAN_HOAN_THANH = 6;

        public static List<StatusDefine> statusBillImportMaterial = new List<StatusDefine>();
        static LoadStatusBillImportMaterial()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = KHOI_TAO,
                Name = "Khởi tạo phiếu"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = XAC_NHAN_CHO_NHAP,
                Name = "Xác nhận phiếu"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = KHAI_BAO_DU_LIEU,
                Name = "Khai báo dữ liệu khách hàng"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = NHAP_KHO,
                Name = "Nhập kho"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = XAC_NHAN_NHAP_LOI,
                Name = "Nhập kho chênh lệch"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = XAC_NHAN_THU_KHO,
                Name = "Nhập kho lỗi thủ kho xác nhận"
            });
            statusBillImportMaterial.Add(new StatusDefine()
            {
                Id = XAC_NHAN_HOAN_THANH,
                Name = "Hoàn thành nhập kho"
            });
        }

    }
    /// <summary>
    /// Phiếu xuất thành phẩm
    /// </summary>
    public static class LoadFunctionBillExportGoodsToCus
    {
        public const int CANCEL = -1;
        public const int CREATE = 0;
        public const int UPDATE = 1;

        public static List<StatusDefine> StatusBillExportGoodToCus = new List<StatusDefine>();
        static LoadFunctionBillExportGoodsToCus()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = CANCEL,
                Name = "Hủy phiếu"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới phiếu"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = UPDATE,
                Name = "Cập nhật phiếu"
            });

        }

    }

    public static class LoadStateBillExportGoodsToCus
    {
        public const int CANCEL = -1;
        public const int CREATE = 0;
        public const int APP = 1;
        public const int EXECUTE = 2;
        public const int COMPLETE = 3;


        public static List<StatusDefine> StatusBillExportGoodToCus = new List<StatusDefine>();
        static LoadStateBillExportGoodsToCus()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = CANCEL,
                Name = "Hủy phiếu"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Khởi tạo"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = APP,
                Name = "Đã phê duyệt"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = EXECUTE,
                Name = "Đang thực hiện"
            });
            StatusBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = COMPLETE,
                Name = "Hoàn thành"
            });
        }

    }

    public static class LoadActionBillExportGoodToCus
    {
        public const int EXPORT = 0;
        public const int VIEW = 1;

        public static List<StatusDefine> ActionBillExportGoodToCus = new List<StatusDefine>();
        static LoadActionBillExportGoodToCus()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            ActionBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = EXPORT,
                Name = "Xuất hàng"
            });
            ActionBillExportGoodToCus.Add(new StatusDefine()
            {
                Id = VIEW,
                Name = "Xem"
            });
          

        }

    }

    public static class LoadStateBillDetailExportGoodToCus
    {
        public const int WAIT = 0;
        public const int FINISH = 1;

        public static List<StatusDefine> StateBilDetailExportGoodToCus = new List<StatusDefine>();
        static LoadStateBillDetailExportGoodToCus()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            StateBilDetailExportGoodToCus.Add(new StatusDefine()
            {
                Id = WAIT,
                Name = "Khỏi tạo"
            });
            StateBilDetailExportGoodToCus.Add(new StatusDefine()
            {
                Id = FINISH,
                Name = "Hoàn thành"
            });


        }

    }

    public static class LoadTypeFPBill
    {
        public const int EXP_CUS  = 6;
        public const int IMP_LOCAL = 7;

        public const int EXP_LOCAL = 11;
        public const int IMP_CUS = 15;
        public static List<StatusDefine> TypeFPBill = new List<StatusDefine>();
        static LoadTypeFPBill()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            TypeFPBill.Add(new StatusDefine()
            {
                Id = EXP_CUS,
                Name = "Xuất hàng thành phẩm"
            });
            TypeFPBill.Add(new StatusDefine()
            {
                Id = IMP_LOCAL,
                Name = "Nhập hàng thành phẩm"
            });
            TypeFPBill.Add(new StatusDefine()
            {
                Id = EXP_LOCAL,
                Name = "Xuất trả thành phẩm - nội bộ"
            });
            TypeFPBill.Add(new StatusDefine()
            {
                Id = IMP_CUS,
                Name = "Nhập lại  thàng phẩm - từ khách"
            });


        }
    }
    /// <summary>
    /// Phiếu xuất linh kiện
    /// </summary>
    public static class LoadFunctionBillExportMaterial
    {
        public const int CANCEL = -1;
        public const int CREATE = 0;
        public const int VIEW = 1;
        public const int UPDATE = 2;
        public static List<StatusDefine> StatusBillExportMaterial = new List<StatusDefine>();
        static LoadFunctionBillExportMaterial()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = CANCEL,
                Name = "Hủy phiếu"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới phiếu"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = VIEW,
                Name = "Xem thông tin phiếu"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = UPDATE,
                Name = "Cập nhật phiếu"
            });

        }
    }


    public static class LoadSubTypeBillExportMaterial
    {
        public const int BOM = 0;
        public const int ARISE = 1;
        public const int FORMING = 2;
        public const int FILE = 3;
        public static List<StatusDefine> SubTypeBillExportMaterial = new List<StatusDefine>();
        static LoadSubTypeBillExportMaterial()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            SubTypeBillExportMaterial.Add(new StatusDefine()
            {
                Id = BOM,
                Name = "Phiếu theo bom"
            });
            SubTypeBillExportMaterial.Add(new StatusDefine()
            {
                Id = ARISE,
                Name = "Phiếu phát sinh"
            });
            SubTypeBillExportMaterial.Add(new StatusDefine()
            {
                Id = FORMING,
                Name = "Phiếu gia công"
            });
            SubTypeBillExportMaterial.Add(new StatusDefine()
            {
                Id = FILE,
                Name = "Phiếu theo File"
            });

        }
    }

    public static class LoadStatusBillExportMaterial
    {
        public const int CANCEL = -1;
        public const int CREATE = 0;
        public const int APP = 1;
        public const int EXPORTED = 2;
        public const int COMPLETE = 3;

        public static List<StatusDefine> StatusBillExportMaterial = new List<StatusDefine>();
        static LoadStatusBillExportMaterial()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = CANCEL,
                Name = "Hủy phiếu"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới phiếu"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = APP,
                Name = "Đã duyệt"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = EXPORTED,
                Name = "Đã xuất"
            });
            StatusBillExportMaterial.Add(new StatusDefine()
            {
                Id = COMPLETE,
                Name = "Hoàn thành"
            });
        }
    }


    /// <summary>
    /// Model
    /// </summary>
    public static class LoadStatusDefineModel
    {
        public const int DELETE = -1;
        public const int CREATE = 0;
        public const int UPDATE = 1;
        public const int LOCK = 2;

        public static List<StatusDefine> statusDefineModel = new List<StatusDefine>();
        static LoadStatusDefineModel()
        {
            //statusDefineCustomers = new List<StatusDefineCustomer>();
            statusDefineModel.Add(new StatusDefine()
            {
                Id = DELETE,
                Name = "Xóa Sản phẩm"
            });
            statusDefineModel.Add(new StatusDefine()
            {
                Id = CREATE,
                Name = "Tạo mới sản phẩm"
            });
            statusDefineModel.Add(new StatusDefine()
            {
                Id = UPDATE,
                Name = "Cập nhật thông tin Sản phẩm"
            });
            statusDefineModel.Add(new StatusDefine()
            {
                Id = LOCK,
                Name = "Khóa sản phẩm"
            });
        }

    }
}
