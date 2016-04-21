using System;

namespace EnronMailConversionUtil
{
    public class EmailProperties
    {
        public string OriginalId { get; set; }
        public string Body { get; set; }
        public string MailBox { get; set; }
        public string SubFolder { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
    }
}