using System;

#nullable enable
namespace MatchingSystem.DataLayer.OldEntities
{
    public class StatisticsMain
    {
        public string? StatName { get; set; }
        public double? StatValue { get; set; }
        public string? StatValue_Str { get; set; }
        public double? StatPercentage { get; set; }

        protected bool Equals(StatisticsMain other)
        {
            return StatName == other.StatName &&
                  // StatValue == other.StatValue && 
                   StatValue_Str == other.StatValue_Str &&
                   Nullable.Equals(StatPercentage, other.StatPercentage);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StatisticsMain) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StatName, StatValue, StatValue_Str, StatPercentage);
        }

        public override string ToString()
        {
            return $"StatName: {StatName}, StatValue_Str: {StatValue_Str}, StatPercentage: {StatPercentage}";
        }
    }
}