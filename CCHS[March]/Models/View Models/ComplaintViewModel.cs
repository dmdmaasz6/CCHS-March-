using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class ComplaintViewModel
    {
        public Complainant complainant { get; set; }
        public Compliant complaint { get; set; }
        public HttpPostedFileBase rep_doc { get; set; }
        public Advisor advisor { get; set; }
        public BodyCorporate body_corporate { get; set; }

    }
}