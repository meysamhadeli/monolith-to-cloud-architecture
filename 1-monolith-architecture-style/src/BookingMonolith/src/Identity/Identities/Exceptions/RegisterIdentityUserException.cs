using BuildingBlocks.Exception;

namespace BookingMonolith.Identity.Identities.Exceptions;

public class RegisterIdentityUserException : AppException
{
    public RegisterIdentityUserException(string error) : base(error)
    {
    }
}
