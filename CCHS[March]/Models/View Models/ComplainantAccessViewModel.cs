using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class ComplainantAccessViewModel
    {

        [Required]
        [Display(Name = "Account Number")]
        public string AccNumber { get; set; }

        [Required]
        [Display(Name = "ID Number")]
        public string National_Id { get; set; }

    }
}