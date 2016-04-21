//using System;

//namespace EnronMailConversionUtil
//{
//    public class PropertiesExtractor
//    {
//        public static readonly string[] ProcessedHeaders =
//        {
//            "Subject",
//            "Date",
//            "To",
//            "X-To",
//            "cc",
//            "X-cc",
//            "bcc",
//            "X-bcc"
//        };

//        public static EmailProperties ExtractProperties(Func<string, string> accessor)
//        {
//            var originalId = accessor("_id");
//            var body = accessor("body");
//            var mailBox = accessor("mailbox");
//            var subFolder = accessor("subFolder");
//            var subject = accessor("Subject");
//            var dateString = accessor("Date");

//            var date = DateParser.ParseDate(dateString);

//            return new EmailProperties
//            {
//                OriginalId = originalId,
//                Body = body,
//                MailBox = mailBox,
//                SubFolder = subFolder,
//                Subject = subject,
//                Date = date
//            };
//        }
//    }
//}