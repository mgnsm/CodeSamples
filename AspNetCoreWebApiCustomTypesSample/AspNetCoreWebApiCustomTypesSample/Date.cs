using System;

namespace AspNetCoreWebApiCustomTypesSample
{
    //NOTE: Operators other than == and != and convenience methods have been omitted for brevity.
    public readonly struct Date : IEquatable<Date>
    {
        public Date(int year, int month, int day)
        {
            if (year < 1 || year > 9999)
                throw new ArgumentOutOfRangeException(nameof(year));
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));
            if (day < 1 || day > DateTime.DaysInMonth(year, month))
                throw new ArgumentOutOfRangeException(nameof(day));

            Year = year;
            Month = month;
            Day = day;
        }

        public readonly int Year { get; }
        public readonly int Month { get; }
        public readonly int Day { get; }

        public readonly bool Equals(Date other) =>
            Year == other.Year && Month == other.Month && Day == other.Day;

        public readonly override bool Equals(object obj) => 
            obj is Date date && Equals(date);

        public readonly override int GetHashCode() => 
            HashCode.Combine(Year.GetHashCode(), 
                Month.GetHashCode(), 
                Day.GetHashCode());

        public readonly override string ToString()
        {
            const string Format = "yyyy-MM-dd";
            return new DateTime(Year, Month, Day).ToString(Format);
        }

        public static bool operator ==(Date a, Date b) => a.Equals(b);

        public static bool operator !=(Date a, Date b) => !a.Equals(b);
    }
}