using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSCMS.Web.Models
{
    //Index view model for grid table
    public class IndexModel
    {
        public int page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string orderBy { get; set; }
        public int orderByAsc { get; set; }
        public bool IsPostBack { get; set; }
        public IndexModel()
        {
            PageSize = 10;
            IsPostBack = false;
            orderByAsc = 1;
        }
    }
}