namespace Mongo2SQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mail")]
    public partial class Mail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mail()
        {
            Attachments = new HashSet<Attachment>();
            Headers = new HashSet<Header>();
            Recipients = new HashSet<Recipient>();
            WordOccurrences = new HashSet<WordOccurrence>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Subject { get; set; }

        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Body { get; set; }

        public string MailBox { get; set; }

        public string SubFolder { get; set; }

        public string OriginalId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Header> Headers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipient> Recipients { get; set; }

        public virtual Sender Sender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WordOccurrence> WordOccurrences { get; set; }
    }
}
