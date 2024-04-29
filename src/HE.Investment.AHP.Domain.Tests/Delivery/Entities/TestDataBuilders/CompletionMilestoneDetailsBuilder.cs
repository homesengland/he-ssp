using System.Globalization;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class CompletionMilestoneDetailsBuilder
{
    private CompletionDate? _completionDate = new(true, "12", "1", "2025");
    private MilestonePaymentDate? _paymentDate = new(true, "1", "7", "2026");

    public CompletionMilestoneDetailsBuilder WithMilestoneDateBeforeProgrammeStartDate()
    {
        _completionDate = new CompletionDate(true, "1", "1", "2022");

        return this;
    }

    public CompletionMilestoneDetailsBuilder WithCompletionDate(DateOnly completionDate)
    {
        _completionDate = new CompletionDate(
            true,
            completionDate.Day.ToString(CultureInfo.InvariantCulture),
            completionDate.Month.ToString(CultureInfo.InvariantCulture),
            completionDate.Year.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public CompletionMilestoneDetailsBuilder WithPaymentDate(DateOnly paymentDate)
    {
        _paymentDate = new MilestonePaymentDate(
            true,
            paymentDate.Day.ToString(CultureInfo.InvariantCulture),
            paymentDate.Month.ToString(CultureInfo.InvariantCulture),
            paymentDate.Year.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public CompletionMilestoneDetailsBuilder WithMissingMilestoneDate()
    {
        _completionDate = null;

        return this;
    }

    public CompletionMilestoneDetailsBuilder WithPaymentDateAfterProgrammeEndDate()
    {
        _paymentDate = new MilestonePaymentDate(true, "1", "1", "2028");

        return this;
    }

    public CompletionMilestoneDetails Build()
    {
        return new CompletionMilestoneDetails(
            _completionDate,
            _paymentDate);
    }
}
