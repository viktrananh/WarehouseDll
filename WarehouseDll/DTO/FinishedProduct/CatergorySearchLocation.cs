using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDll.DTO.FinishedProduct
{
    public class CatergorySearchLocation
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class LoadCatergorySearchLocation
    {
        private LoadCatergorySearchLocation instance;
        public LoadCatergorySearchLocation Instance
        {
            get { if (instance == null) instance = new LoadCatergorySearchLocation(); return instance; }
            set { instance = value; }
        }

        public LoadCatergorySearchLocation() { }
        public static List<CatergorySearchLocation> LoadCatergorys()
        {
            List<CatergorySearchLocation> ls = new List<CatergorySearchLocation>()
            {
                new CatergorySearchLocation{ID = 0, Name = "BOX"},
                new CatergorySearchLocation{ID = 1,Name = "LOACATION"},
                new CatergorySearchLocation{ID = 2,Name = "WORK"},
                new CatergorySearchLocation{ID = 3,Name = "MODEL"},
                 new CatergorySearchLocation{ID = 4,Name = "CUSTOMER"},

            };
            return ls;
        }

    }
}
