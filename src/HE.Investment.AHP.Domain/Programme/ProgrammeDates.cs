using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Programme;

public class ProgrammeDates : ValueObject
{
    public ProgrammeDates(
        DateOnly programmeStartDate,
        DateOnly programmeEndDate,
        DateOnly? startOnSiteStartDate = null,
        DateOnly? startOnSiteEndDate = null,
        DateOnly? completionStartDate = null,
        DateOnly? completionEndDate = null)
    {
        ProgrammeStartDate = programmeStartDate;
        ProgrammeEndDate = programmeEndDate;
        if (programmeStartDate > programmeEndDate)
        {
            throw new DomainValidationException(nameof(ProgrammeStartDate), "Programme end date should be after start date.");
        }

        StartOnSiteStartDate = startOnSiteStartDate;
        StartOnSiteEndDate = startOnSiteEndDate;
        CompletionStartDate = completionStartDate;
        CompletionEndDate = completionEndDate;
    }

    public DateOnly ProgrammeStartDate { get; }

    public DateOnly ProgrammeEndDate { get; }

    public DateOnly? StartOnSiteStartDate { get; }

    public DateOnly? StartOnSiteEndDate { get; }

    public DateOnly? CompletionStartDate { get; }

    public DateOnly? CompletionEndDate { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ProgrammeStartDate;
        yield return ProgrammeEndDate;
        yield return StartOnSiteStartDate;
        yield return StartOnSiteEndDate;
        yield return CompletionStartDate;
        yield return CompletionEndDate;
    }
}
