using System;

namespace cursework.Models;

public class Rating
{
    private double _rating;
    public double Rate
    {
        get => this._rating;
        set
        {
            if (value < 0 || value > 10 || Math.Round(value * 10) % 1 != 0)
            {
                throw new ArgumentOutOfRangeException($"Rating must be between 0 and 10 and have no more than 1 decimal place, but received {value}.");
            }
            this._rating = value;
        }
    }

    public Rating(double rate)
    {
        this.Rate = rate;
    }

    public override string ToString()
    {
        return $"{this.Rate}/10";
    }
    
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Rating other = (Rating)obj;

        var rate = this.Rate == other.Rate;
        return rate;
    }
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(this.Rate);
        return hash.ToHashCode();
    }
    
    public static bool operator ==(Rating? obj1, Rating? obj2)
    {
        if (ReferenceEquals(obj1, obj2)) 
            return true;
        if (ReferenceEquals(obj1, null)) 
            return false;
        if (ReferenceEquals(obj2, null))
            return false;
        return obj1.Equals(obj2);
    }
    public static bool operator !=(Rating? obj1, Rating? obj2) => !(obj1 == obj2);
}
