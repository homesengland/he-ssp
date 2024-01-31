using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class LandRegistryDetails : ValueObject, IQuestion
{
    public LandRegistryDetails(
        bool? isLandRegistryTitleNumberRegistered,
        LandRegistryTitleNumber? titleNumber,
        bool? isGrantFundingForAllHomesCoveredByTitleNumber)
    {
        if ((isLandRegistryTitleNumberRegistered == null || !isLandRegistryTitleNumberRegistered.Value) &&
            (titleNumber.IsProvided() || isGrantFundingForAllHomesCoveredByTitleNumber.IsProvided()))
        {
            throw new DomainValidationException(nameof(LandRegistryTitleNumber), "Cannot set Land Registry title number if number is not registered.");
        }

        IsLandRegistryTitleNumberRegistered = isLandRegistryTitleNumberRegistered;
        TitleNumber = titleNumber;
        IsGrantFundingForAllHomesCoveredByTitleNumber = isGrantFundingForAllHomesCoveredByTitleNumber;
    }

    public bool? IsLandRegistryTitleNumberRegistered { get; }

    public LandRegistryTitleNumber? TitleNumber { get; }

    public bool? IsGrantFundingForAllHomesCoveredByTitleNumber { get; }

    public static LandRegistryDetails WithIsRegistered(LandRegistryDetails? landRegistry, bool? isLandRegistryTitleNumberRegistered) =>
        new(isLandRegistryTitleNumberRegistered, landRegistry?.TitleNumber, landRegistry?.IsGrantFundingForAllHomesCoveredByTitleNumber);

    public static LandRegistryDetails WithDetails(
        LandRegistryDetails? landRegistry,
        LandRegistryTitleNumber? titleNumber,
        bool? isGrantFundingForAllHomesCoveredByTitleNumber) =>
        new(landRegistry?.IsLandRegistryTitleNumberRegistered, titleNumber, isGrantFundingForAllHomesCoveredByTitleNumber);

    public bool IsAnswered()
    {
        return IsLandRegistryTitleNumberRegistered == false ||
               (IsLandRegistryTitleNumberRegistered == true && TitleNumber.IsProvided() && IsGrantFundingForAllHomesCoveredByTitleNumber.IsProvided());
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsLandRegistryTitleNumberRegistered;
        yield return TitleNumber;
        yield return IsGrantFundingForAllHomesCoveredByTitleNumber;
    }
}
