using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class ManageViewModel
    {
        public Complainant complainant { get; set; }
        public Compliant complaint { get; set; }
        public HttpPostedFileBase rep_doc { get; set; }
        public Advisor advisor { get; set; }
        public BodyCorporate body_corporate { get; set; }
        public List<UserViewModel> assesors { get; set; }
    }

    public class UserViewModel
    {
        [Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
    }
}