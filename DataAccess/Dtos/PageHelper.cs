using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos
{
    public class PageHelper
    {
        public int pageCurrent { get; set; }
        public int totalPages { get; set; }

        public string url { get; set; }
    }
}
