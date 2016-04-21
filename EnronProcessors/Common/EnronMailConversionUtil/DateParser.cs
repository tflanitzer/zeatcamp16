using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnronMailConversionUtil
{
    public class DateParser
    {
        private static readonly Regex DateRegex = new Regex(@"(?<dayofweek>\w{3}), (?<relevantPart>(?<day>\d+) (?<month>\w+) (?<year>\d{4}) (?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}) (?<tzplusminus>[+-])(?<tzhours>\d{2})(?<tzminutes>\d{2})).*");

        public static DateTime ParseDate(string dateString)
        {
            var match = DateRegex.Match(dateString);

            if (!match.Success)
            {
                Console.WriteLine("Date did not match: '" + dateString + "'.");
                return DateTime.MinValue;
            }

            var relevantPart = match.Groups["relevantPart"].Value;
            relevantPart = relevantPart.Insert(relevantPart.Length - 2, ":");

            DateTime date;
            DateTime.TryParseExact(
                relevantPart,
                "d MMM yyyy HH:mm:ss zzz",
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out date);

            // Correct dates with year 0001 to year 2000 (best guess)
            if (date.Year == 1)
            {
                date = date.AddYears(1999);
            }

            if (date == DateTime.MinValue)
                Console.WriteLine("Date was in unexpected format : '" + dateString + "'.");

            return date;
        }
    }
}
