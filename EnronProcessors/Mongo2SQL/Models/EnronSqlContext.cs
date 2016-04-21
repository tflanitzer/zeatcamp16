namespace Mongo2SQL.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EnronSqlContext : DbContext
    {
        public EnronSqlContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<EmailAccount> EmailAccount { get; set; }
        public virtual DbSet<Header> Header { get; set; }
        public virtual DbSet<Mail> Mail { get; set; }
        public virtual DbSet<Recipient> Recipient { get; set; }
        public virtual DbSet<Sender> Sender { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<WordOccurrence> WordOccurrence { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailAccount>()
                .HasMany(e => e.Recipient)
                .WithRequired(e => e.EmailAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmailAccount>()
                .HasMany(e => e.Sender)
                .WithRequired(e => e.EmailAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mail>()
                .HasMany(e => e.Attachments)
                .WithRequired(e => e.Mail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mail>()
                .HasMany(e => e.Headers)
                .WithRequired(e => e.Mail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mail>()
                .HasMany(e => e.Recipients)
                .WithRequired(e => e.Mail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mail>()
                .HasOptional(e => e.Sender)
                .WithRequired(e => e.Mail);

            modelBuilder.Entity<Mail>()
                .HasMany(e => e.WordOccurrences)
                .WithRequired(e => e.Mail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Word>()
                .HasMany(e => e.WordOccurrence)
                .WithRequired(e => e.Word)
                .WillCascadeOnDelete(false);
        }
    }
}
