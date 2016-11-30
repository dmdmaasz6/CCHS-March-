using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class SubmitResponseViewModel
    {

        public Customer_Response response { get; set; }
        public List<HttpPostedFileBase> attachments { get; set; }

    }
}