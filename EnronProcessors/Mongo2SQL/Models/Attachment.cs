namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attachment")]
    public partial class Attachment
    {
        public int Id { get; set; }

        public int MailId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Mail Mail { get; set; }
    }
}
