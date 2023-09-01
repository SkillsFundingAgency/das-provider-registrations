namespace SFA.DAS.ProviderRegistrations.Services;

public interface IEmployerApprenticeshipService
{
    Task<bool> IsEmailAddressInUse(string emailAddress, CancellationToken cancellationToken = default);
}