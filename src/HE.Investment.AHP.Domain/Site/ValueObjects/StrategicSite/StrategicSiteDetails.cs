using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;

public class StrategicSiteDetails : ValueObject, IQuestion
{
    public StrategicSiteDetails(bool? isStrategicSite = null, StrategicSiteName? siteName = null)
    {
        if (isStrategicSite != true && siteName.IsProvided())
        {
            throw new DomainValidationException("Strategic site name can be provided only for strategic site.");
        }

        IsStrategicSite = isStrategicSite;
        SiteName = siteName;
    }

    public bool? IsStrategicSite { get; }

    public StrategicSiteName? SiteName { get; }

    public static StrategicSiteDetails Create(bool? isStrategicSite = null, string? siteName = null)
    {
        if (isStrategicSite == null)
        {
            throw new DomainValidationException(nameof(IsStrategicSite), "Select if this is strategic site");
        }

        if (!isStrategicSite.Value)
        {
            return new StrategicSiteDetails(isStrategicSite.Value);
        }

        var operationResult = OperationResult.New();
        var name = operationResult.AggregateNullable(() => new StrategicSiteName(siteName));
        var details = operationResult.AggregateNullable(() => new StrategicSiteDetails(isStrategicSite, name));

        operationResult.CheckErrors();

        return details!;
    }

    public bool IsAnswered()
    {
        return IsStrategicSite is false || (IsStrategicSite is true && SiteName.IsProvided());
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsStrategicSite;
        yield return SiteName;
    }
}
