using System.Text.Json.Serialization;

namespace DataManagerTests.Entities.Postgres;

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("age")]
    public int Age { get; set; }
    [JsonPropertyName("total_wealth")]
    public double TotalWealth { get; set; }
    [JsonPropertyName("measurement_date")]
    public DateTime MeasurementDate { get; set; }



    #region ObjectComparison
    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (ReferenceEquals(obj, this)) return false;

        if (obj.GetType() != this.GetType()) return false;

        var item = obj as Tag;
        return this.Name == item.Name
               && this.Age == item.Age
               && Math.Abs(this.TotalWealth - item.TotalWealth) < 0.1
               && this.MeasurementDate == item.MeasurementDate;
    }

    public static bool operator ==(Tag lhs, Tag rhs)
    {
        return Equals(lhs, rhs);
    }

    public static bool operator !=(Tag lhs, Tag rhs)
    {
        return !Equals(lhs, rhs);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }
    #endregion
}