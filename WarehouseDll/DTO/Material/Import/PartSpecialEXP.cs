using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.Material.Import
{
    public class PartSpecialEXP
    {
        public string PartNumber { get; set; }
        public string DateEXP { get; set; }
        public int Qty { get; set; }
        public int QtyReal { get; set; }
        public int TypeSpecial { get; set; }

        public PartSpecialEXP() { }

        public PartSpecialEXP(DataRow row)
        {
            PartNumber = row["PART_NUMBER"].ToString();
            DateEXP = row["DATE_EXP"].ToString();
            Qty = int.Parse(row["QTY_ALLOWED"].ToString());
            QtyReal = int.Parse(row["QTY_REAL"].ToString());
            TypeSpecial = int.Parse(row["TYPE_SPECIAL"].ToString());

        }

    }
}
