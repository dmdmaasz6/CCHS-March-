using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models.View_Models
{
    public class ComplainantViewModel
    {
        public string AccountNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string PO_Box { get; set; }

        [Required]
        public string Tel_Number { get; set; }

        [Required]
        public string FaxNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Id_Number { get; set; }
    }
}