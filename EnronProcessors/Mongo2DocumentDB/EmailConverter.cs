using System;
using System.Linq;
using EnronMailConversionUtil;
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

        //public Mail ConvertEmail(BsonDocument sourceEmail)
        //{
        //    var mail = new Mail();
        //    var headers = sourceEmail["headers"].AsBsonDocument;

        //    FillMailProperties(mail, sourceEmail, headers);
        //    AddSender(mail, headers);
        //    AddRecipients(mail, headers);
        //    AddRemainingHeaders(mail, headers);

        //    return mail;
        //}

        //private static void FillMailProperties(Mail mail, BsonDocument sourceEmail, BsonDocument headers)
        //{
        //    var originalId = sourceEmail["_id"].AsObjectId.ToString();
        //    var body = sourceEmail["body"];
        //    var mailBox = sourceEmail["mailbox"];
        //    var subFolder = sourceEmail["subFolder"];
        //    var subject = headers["Subject"];
        //    var dateString = headers["Date"].AsString;

        //    var date = DateParser.ParseDate(dateString);

        //    mail.OriginalId = originalId;
        //    mail.Body = body.AsString;
        //    mail.MailBox = mailBox.AsString;
        //    mail.SubFolder = subFolder.AsString;
        //    mail.Subject = subject.AsString;
        //    mail.Date = new DateTimeOffset(date).ToUnixTimeSeconds();
        //}

        //private void AddSender(Mail mail, BsonDocument headers)
        //{
        //    var from = headers.GetValue("From");
        //    var xFrom = headers.GetValue("X-From", null);

        //    var sender = new Sender { EmailAccountId = EmailAccountProvider.GetEmailAccount(from.AsString).Id };

        //    if (xFrom != null)
        //    {
        //        sender.Name = xFrom.AsString;
        //    }

        //    mail.Sender = sender;
        //}

        //private void AddRecipients(Mail mail, BsonDocument headers)
        //{
        //    try
        //    {
        //        var to = headers.GetValue("To", null)?.AsString;
        //        var xTo = headers.GetValue("X-To", null)?.AsString;
        //        var cc = headers.GetValue("Cc", null)?.AsString;
        //        var xCc = headers.GetValue("X-cc", null)?.AsString;
        //        var bcc = headers.GetValue("Bcc", null)?.AsString;
        //        var xBcc = headers.GetValue("X-bcc", null)?.AsString;

        //        var parsedRecipients = RecipientParser.ParseRecipients(to, xTo, cc, xCc, bcc, xBcc);

        //        foreach (var parsedRecipient in parsedRecipients)
        //        {
        //            mail.Recipients.Add(new Recipient
        //            {
        //                EmailAccountId = EmailAccountProvider.GetEmailAccount(parsedRecipient.EmailAddress).Id,
        //                Name = parsedRecipient.Name,
        //                Type = parsedRecipient.RecipientType
        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception occurred while adding recipients from header: " + headers.ToString() + "\r\nException: " + e.ToString());

        //        throw;
        //    }
        //}

        //private void AddRemainingHeaders(Mail mail, BsonDocument headers)
        //{
        //    var remainingHeaders = headers.Where(_ => !ProcessedHeaders.Contains(_.Name));

        //    foreach (var remainingHeader in remainingHeaders)
        //    {
        //        var header = new Header
        //        {
        //            Key = remainingHeader.Name,
        //            Value = remainingHeader.Value.AsString
        //        };

        //        mail.Headers.Add(header);
        //    }
        //}
    }
}