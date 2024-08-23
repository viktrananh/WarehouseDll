using ROSE_Dll.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseDll.DTO.Material
{
    public class BillInfor
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string Part { get; set; }
        public int Request { get; set; }
        public int Thoery { get; set; }
        public int Real { get; set; }
        public string Unit { get; set; }

        public BillInfor () {  }

        public BillInfor(DataRow row)
        {
            Id = int.Parse(row["ID"].ToString());
            BillNumber = row["BILL_NUMBER"].ToString();
            Part = row["PART"].ToString();
            Request = int.Parse(row["REQUEST"].ToString());
            Thoery = int.Parse(row["THEORY"].ToString());
            Real = int.Parse(row["REAL"].ToString());
        }
    }
}
