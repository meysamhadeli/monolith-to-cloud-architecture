using BookingMonolith.Passenger.Passengers.Exceptions;

namespace BookingMonolith.Passenger.Passengers.ValueObjects;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Name Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidNameException();
        }

        return new Name(value);
    }

    public static implicit operator string(Name name)
    {
        return name.Value;
    }
}
