namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Header")]
    public partial class Header
    {
        public int Id { get; set; }

        public int MailId { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        public string Value { get; set; }

        public virtual Mail Mail { get; set; }
    }
}
