namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WordOccurrence")]
    public partial class WordOccurrence
    {
        public int Id { get; set; }

        public int MailId { get; set; }

        public int WordId { get; set; }

        public int? Position { get; set; }

        public virtual Mail Mail { get; set; }

        public virtual Word Word { get; set; }
    }
}
