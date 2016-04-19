using System;
using Mongo2SQL.Models;
using MongoDB.Bson;

namespace Mongo2SQL
{
    public class EmailConverter
    {
        public IEmailAccountProvider EmailAccountProvider { get; set; }

        public EmailConverter(IEmailAccountProvider emailAccountProvider)
        {
            EmailAccountProvider = emailAccountProvider;
        }

        public Mail ConvertEmail(BsonDocument sourceEmail)
        {
            var mail = new Mail();
            var headers = sourceEmail["headers"].AsBsonDocument;

            FillMailProperties(mail, sourceEmail, headers);

            var senderEmailAddress = headers["From"];
            var senderName = headers.GetValue("X-From");

            mail.Sender = new Sender {EmailAccount = EmailAccountProvider.GetEmailAccount(senderEmailAddress.AsString)};


            var to = headers["To"];
            var xTo = headers["X-To"];

            var cc = headers.GetValue("Cc", null);
            var xCc = headers.GetValue("X-cc", null);

            var bcc = headers.GetValue("Bcc", null);
            var xBcc = headers.GetValue("X-bcc", null);

            return mail;
        }

        private static void FillMailProperties(Mail mail, BsonDocument sourceEmail, BsonDocument headers)
        {
            var body = sourceEmail["body"];
            var subFolder = sourceEmail["subFolder"];
            var subject = headers["Subject"];
            var dateString = headers["Date"];

            mail.Body = body.AsString;
            mail.SubFolder = subFolder.AsString;
            mail.Subject = subject.AsString;
            mail.Date = new DateTimeOffset(dateString.ToUniversalTime()).ToUnixTimeSeconds();
        }
    }
}