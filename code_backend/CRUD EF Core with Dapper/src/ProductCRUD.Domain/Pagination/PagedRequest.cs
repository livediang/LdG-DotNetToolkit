using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCRUD.Domain.Pagination
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? SortBy { get; set; } = "CreatedAt";
        public bool SortDesc { get; set; } = true;
    }
}
