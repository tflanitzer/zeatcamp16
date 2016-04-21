namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sender")]
    public partial class Sender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MailId { get; set; }

        public int EmailAccountId { get; set; }

        public string Name { get; set; }

        public virtual EmailAccount EmailAccount { get; set; }

        public virtual Mail Mail { get; set; }
    }
}
