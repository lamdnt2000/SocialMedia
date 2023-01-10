using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Pagination
{
    public class PaginationList<T> { 
       

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}
