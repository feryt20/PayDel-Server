using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 0;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > 50) ? 50 : value; }
        }
        public string Filter { get; set; }

        //SortHeader
        public string SortHe { get; set; }
        //SortDirection
        public string SortDir { get; set; }
    }
}
