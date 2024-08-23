using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseDll.DTO
{
    public class CycleReport
    {
        public const string TAT_CA = "Tất cả";
        public const string HOM_NAY = "Hôm nay";
        public const string THANG_NAY = "Tháng này";
        public const string TU_DAU_THANG_TOI_NAY = "Từ đầu tháng đến hiện tại";
        public const string QUY_NAY = "Quý này";
        public const string TU_DAU_QUY_TOI_NAY = "Từ đầu quý đến hiện tại";
        public const string NAM_NAY = "Năm nay";
        public const string TU_DAU_NAM_TOI_NAY = "Từ đầu năm đến hiện tại";
        public const string QUY_I = "Quý I";
        public const string QUY_II = "Quý II";
        public const string QUY_III = "Quý III";
        public const string QUY_IV = "Quý IV";
        public const string THANG_1 = "Tháng 1";
        public const string THANG_2 = "Tháng 2";
        public const string THANG_3 = "Tháng 3";
        public const string THANG_4 = "Tháng 4";
        public const string THANG_5 = "Tháng 5";
        public const string THANG_6 = "Tháng 6";
        public const string THANG_7 = "Tháng 7";
        public const string THANG_8 = "Tháng 8";
        public const string THANG_9 = "Tháng 9";
        public const string THANG_10 = "Tháng 10";
        public const string THANG_11 = "Tháng 11";
        public const string THANG_12 = "Tháng 12";

        public List<string> Cycle()
        {
            List<string> cycles = new List<string> { };
            cycles.Add(TAT_CA);
            cycles.Add(HOM_NAY);
            cycles.Add(THANG_NAY);
            cycles.Add(TU_DAU_THANG_TOI_NAY);
            cycles.Add(TU_DAU_QUY_TOI_NAY);
            cycles.Add(NAM_NAY);
            cycles.Add(TU_DAU_NAM_TOI_NAY);
            cycles.Add(THANG_1);
            cycles.Add(THANG_2);
            cycles.Add(THANG_3);
            cycles.Add(THANG_4);
            cycles.Add(THANG_5);
            cycles.Add(THANG_6);
            cycles.Add(THANG_7);
            cycles.Add(THANG_8);
            cycles.Add(THANG_9);
            cycles.Add(THANG_10);
            cycles.Add(THANG_11);
            cycles.Add(THANG_12);
            return cycles;
        }
        public FillDate FillComboboxByCyle(ComboBox cbx)
        {
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (cbx.SelectedItem.ToString() == CycleReport.TAT_CA)
            {
                start = new DateTime(1990, 1, 1);
                end = new DateTime(2990, 12, 31);
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.HOM_NAY)
            {
                start = DateTime.Today;
                end = DateTime.Today.AddDays(1);
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.TU_DAU_THANG_TOI_NAY)
            {

                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end = DateTime.Today.AddDays(1);
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_NAY)
            {

                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.TU_DAU_NAM_TOI_NAY)
            {

                start = new DateTime(DateTime.Now.Year, 1, 1);
                end  = DateTime.Today.AddDays(1);
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.NAM_NAY)
            {

                start = new DateTime(DateTime.Now.Year, 1, 1);
                end = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_1)
            {

                start = new DateTime(DateTime.Now.Year, 1, 1);
                end = new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_2)
            {

                start = new DateTime(DateTime.Now.Year, 2, 1);
                end = new DateTime(DateTime.Now.Year, 2, DateTime.DaysInMonth(DateTime.Now.Year, 2));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_3)
            {

                start = new DateTime(DateTime.Now.Year, 3, 1);
                end = new DateTime(DateTime.Now.Year, 3, DateTime.DaysInMonth(DateTime.Now.Year, 3));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_4)
            {

                start = new DateTime(DateTime.Now.Year, 4, 1);
                end = new DateTime(DateTime.Now.Year, 4, DateTime.DaysInMonth(DateTime.Now.Year, 4));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_5)
            {

                start = new DateTime(DateTime.Now.Year, 5, 1);
                end = new DateTime(DateTime.Now.Year, 5, DateTime.DaysInMonth(DateTime.Now.Year, 5));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_6)
            {

                start = new DateTime(DateTime.Now.Year, 6, 1);
                end = new DateTime(DateTime.Now.Year, 6, DateTime.DaysInMonth(DateTime.Now.Year, 6));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_7)
            {

                start = new DateTime(DateTime.Now.Year, 7, 1);
                end = new DateTime(DateTime.Now.Year, 7, DateTime.DaysInMonth(DateTime.Now.Year, 7));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_8)
            {

                start = new DateTime(DateTime.Now.Year, 8, 1);
                end = new DateTime(DateTime.Now.Year, 8, DateTime.DaysInMonth(DateTime.Now.Year, 8));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_9)
            {

                start = new DateTime(DateTime.Now.Year, 9, 1);
                end = new DateTime(DateTime.Now.Year, 9, DateTime.DaysInMonth(DateTime.Now.Year, 9));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_10)
            {

                start = new DateTime(DateTime.Now.Year, 10, 1);
                end = new DateTime(DateTime.Now.Year, 10, DateTime.DaysInMonth(DateTime.Now.Year, 10));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else if (cbx.SelectedItem.ToString() == CycleReport.THANG_11)
            {

                start = new DateTime(DateTime.Now.Year, 11, 1);
                end = new DateTime(DateTime.Now.Year, 11, DateTime.DaysInMonth(DateTime.Now.Year, 11));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
            else
            {

                start = new DateTime(DateTime.Now.Year, 12, 1);
                end = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
                FillDate fill = new FillDate()
                {
                    StartTime = start,
                    EndTime = end,
                };
                return fill;
            }
        }
    }


    public class FillDate
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
