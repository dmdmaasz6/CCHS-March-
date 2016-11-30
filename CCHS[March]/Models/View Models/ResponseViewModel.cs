using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class ResponseViewModel
    {

        public List<Customer_Response> list { get; set; }
        public int complaint_id { get; set; }

    }
}