//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipient
    {
        public int Id { get; set; }
        public int EmailAccountId { get; set; }
        public int MailId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    
        public virtual EmailAccount EmailAccount { get; set; }
        public virtual Mail Mail { get; set; }
    }
}
