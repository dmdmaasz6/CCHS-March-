//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCHS_March_.Models.Data_Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer_Response
    {
        public Customer_Response()
        {
            this.Attachments = new HashSet<Attachments>();
        }
    
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message_Body { get; set; }
        public System.DateTime Date_Created { get; set; }
        public long Parent_Message_Id { get; set; }
        public string From { get; set; }
    
        public virtual Compliant Compliant { get; set; }
        public virtual ICollection<Attachments> Attachments { get; set; }
    }
}
