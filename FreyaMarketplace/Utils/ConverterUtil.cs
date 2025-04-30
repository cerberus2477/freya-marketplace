using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Utils
{
    public static class ConverterUtil
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            string result;
            var now = DateTime.UtcNow;
            var diff = now - dateTime;

            if (diff.TotalSeconds < 60)
                result = "egy perce";
            else if (diff.TotalMinutes < 60)
                result = $"{(int)diff.TotalMinutes} perce";
            else if (diff.TotalHours < 2)
                result = "egy órája";
            else if (diff.TotalHours < 24)
                result = $"{(int)diff.TotalHours} órája";
            else if (diff.TotalDays < 2)
                result = "tegnap";
            else if (diff.TotalDays < 7)
                result = $"{(int)diff.TotalDays} napja";
            else if (diff.TotalDays < 14)
                result = "egy hete";
            else if (diff.TotalDays < 30)
                result = $"{(int)(diff.TotalDays / 7)} hete";
            else
                // Older than a month — return absolute date and time in "yyyy.MM.dd HH:mm"
                result = dateTime.ToLocalTime().ToString("yyyy.MM.dd HH:mm");

            return result;
        }

        public static string GetRelativeDateTime(DateTime dateTime)
        {
            var local = dateTime.ToLocalTime();
            var now = DateTime.Now;

            string datePart = "";
            if (local.Date != now.Date)
            {
                var format = local.Year != now.Year ? "yyyy. MMMM d." : "MMMM d.";
                datePart = local.ToString(format + " ");
            }

            string timePart = local.ToString("HH:mm");
            string relative = GetRelativeTime(dateTime);

            return $"{datePart}{timePart} ({relative})";
        }

    }
}
