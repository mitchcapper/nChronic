using System;
using System.Globalization;

namespace Chronic
{
    public class Options
    {
        public static readonly int DefaultAmbiguousTimeRange = 6;

        public Func<DateTime> Clock { get; set; }

        public Pointer.Type Context { get; set; }

        public int AmbiguousTimeRange { get; set; } //the range is AmbiguousTimeRange AM to AmbiguousTimeRange PM that ambiguous times (like 5:00) are expected within

        public EndianPrecedence EndianPrecedence { get; set; }

        public string OriginalPhrase { get; set; }
        public DayOfWeek FirstDayOfWeek { get; set; }

        public Options()
        {
            AmbiguousTimeRange = DefaultAmbiguousTimeRange;
            EndianPrecedence = EndianPrecedence.Middle;
            Clock = () => DateTime.Now;
            FirstDayOfWeek = DayOfWeek.Sunday;
        }
    }
}