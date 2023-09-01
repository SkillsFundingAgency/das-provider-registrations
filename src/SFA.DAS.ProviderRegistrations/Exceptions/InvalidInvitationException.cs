using System;

namespace SFA.DAS.ProviderRegistrations.Exceptions;

public class InvalidInvitationException : Exception
{
    public InvalidInvitationException(string message) : base(message)
    {

    }
}