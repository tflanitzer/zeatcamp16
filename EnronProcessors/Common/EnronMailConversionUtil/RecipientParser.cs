using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnronMailConversionUtil
{
    public class RecipientParser
    {
        public static ParsedRecipient[] ParseRecipients(string to, string xTo, string cc, string xCc, string bcc, string xBcc)
        {
            var toRecipients = ParseRecipientsOfType(to, xTo, RecipientType.To);
            var ccRecipients = ParseRecipientsOfType(to, xTo, RecipientType.Cc);
            var bccRecipients = ParseRecipientsOfType(to, xTo, RecipientType.Bcc);

            return toRecipients.Concat(ccRecipients).Concat(bccRecipients).ToArray();
        }

        private static IEnumerable<ParsedRecipient> ParseRecipientsOfType(string header, string xHeader, string recipientType)
        {
            if (header == null)
                return new ParsedRecipient[0];

            string[] splitXHeader = null;

            if (!string.IsNullOrWhiteSpace(xHeader))
            {
                splitXHeader = xHeader.Split(',').Select(_ => _.Trim()).ToArray();
            }

            var splitHeader = header.Split(',').Select(_ => _.Trim()).ToArray();

            return splitHeader.Select((emailAddress, i) =>
            {
                var recipient = new ParsedRecipient()
                {
                    EmailAddress = emailAddress,
                    RecipientType = recipientType
                };

                if (splitXHeader != null && splitXHeader.Length == splitHeader.Length)
                    recipient.Name = splitXHeader[i];

                return recipient;
            });
        }
    }
}
