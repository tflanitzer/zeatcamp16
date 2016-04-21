namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recipient")]
    public partial class Recipient
    {
        public int Id { get; set; }

        public int EmailAccountId { get; set; }

        public int MailId { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        public string Type { get; set; }

        public virtual EmailAccount EmailAccount { get; set; }

        public virtual Mail Mail { get; set; }
    }
}
