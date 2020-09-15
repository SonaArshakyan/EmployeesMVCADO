using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum SortDirection { Asc, Desc }
    public class SearchModel
    {
        public string SearchBy { get; set; }
        public string Search { get; set; }
        public string SearchOrderBy { get; set; }
        public SortDirection? sort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}
