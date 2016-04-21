using System;
using System.Globalization;
using System.Linq;
using Mongo2DocumentDB.Models;
using MongoDB.Bson;

namespace Mongo2DocumentDB
{
    public class EmailConverter
    {
        public readonly string[] ProcessedHeaders = 
        {
            "Subject",
            "Date",
            "To",
            "X-To",
            "cc",
            "X-cc",
            "bcc",
            "X-bcc"
        };

        public IEmailAccountProvider EmailAccountProvider { get; set; }

        public EmailConverter(IEmailAccountProvider emailAccountProvider)
        {
            EmailAccountProvider = emailAccountProvider;
        }

        public dynamic ConvertEmail(BsonDocument sourceEmail)
        {
            var mail = new Mail();
            var headers = sourceEmail["headers"].AsBsonDocument;

            FillMailProperties(mail, sourceEmail, headers);
            AddSender(mail, headers);
            AddRecipients(mail, headers);
            AddRemainingHeaders(mail, headers);

            return mail;
        }

        private static void FillMailProperties(Mail mail, BsonDocument sourceEmail, BsonDocument headers)
        {
            var originalId = sourceEmail["_id"].AsObjectId.ToString();
            var body = sourceEmail["body"];
            var mailBox = sourceEmail["mailbox"];
            var subFolder = sourceEmail["subFolder"];
            var subject = headers["Subject"];
            var dateString = headers["Date"].AsString;

            var date = ParseDate(dateString);

            mail.OriginalId = originalId;
            mail.Body = body.AsString;
            mail.MailBox = mailBox.AsString;
            mail.SubFolder = subFolder.AsString;
            mail.Subject = subject.AsString;
            mail.Date = new DateTimeOffset(date).ToUnixTimeSeconds();
        }

        private static DateTime ParseDate(string dateString)
        {
            DateTime date;

            dateString = dateString.Substring(dateString.IndexOf(",") + 1);
            dateString = dateString.Substring(0, dateString.LastIndexOf("("));
            dateString = dateString.Trim();
            dateString = dateString.Insert(dateString.Length - 2, ":");

            DateTime.TryParseExact(
                dateString,
                "d MMM yyyy HH:mm:ss zzz",
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out date);

            if (date == DateTime.MinValue)
                Console.WriteLine("Date was in unexpected format : '" + dateString + "'.");

            return date;
        }

        private void AddSender(Mail mail, BsonDocument headers)
        {
            var from = headers.GetValue("From");
            var xFrom = headers.GetValue("X-From", null);

            var sender = new Sender { EmailAccountId = EmailAccountProvider.GetEmailAccount(from.AsString).Id };

            if (xFrom != null)
            {
                sender.Name = xFrom.AsString;
            }

            mail.Sender = sender;
        }

        private void AddRecipients(Mail mail, BsonDocument headers)
        {
            try
            {
                AddRecipients(mail, headers.GetValue("To", null), headers.GetValue("X-To", null), RecipientType.To);
                AddRecipients(mail, headers.GetValue("Cc", null), headers.GetValue("X-cc", null), RecipientType.Cc);
                AddRecipients(mail, headers.GetValue("Bcc", null), headers.GetValue("X-bcc", null), RecipientType.Bcc);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while adding recipients from header: " + headers.ToString() + "\r\nException: " + e.ToString());
                
                throw;
            }
        }

        private void AddRecipients(Mail mail, BsonValue header, BsonValue xHeader, string recipientType)
        {
            if (header != null)
            {
                string[] splitXHeader = null;

                if (xHeader != null && !string.IsNullOrWhiteSpace(xHeader.AsString))
                {
                    splitXHeader = xHeader.AsString.Split(',').Select(_ => _.Trim()).ToArray();
                }

                var splitHeader = header.AsString.Split(',').Select(_ => _.Trim()).ToArray();

                for (var i = 0; i < splitHeader.Length; i++)
                {
                    var to = splitHeader[i];

                    var recipient = new Recipient()
                    {
                        EmailAccountId = EmailAccountProvider.GetEmailAccount(to).Id,
                        Type = recipientType
                    };

                    if (splitXHeader != null && splitXHeader.Length == splitHeader.Length)
                    {
                        recipient.Name = splitXHeader[i];
                    }

                    mail.Recipients.Add(recipient);
                }
            }
        }

        private void AddRemainingHeaders(Mail mail, BsonDocument headers)
        {
            var remainingHeaders = headers.Where(_ => !ProcessedHeaders.Contains(_.Name));

            foreach (var remainingHeader in remainingHeaders)
            {
                var header = new Header
                {
                    Key = remainingHeader.Name,
                    Value = remainingHeader.Value.AsString
                };

                mail.Headers.Add(header);
            }
        }
    }
}