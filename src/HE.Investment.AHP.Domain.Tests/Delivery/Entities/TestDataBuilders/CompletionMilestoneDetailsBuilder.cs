using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class CompletionMilestoneDetailsBuilder
{
    private CompletionDate? _completionDate = new("12", "1", "2025");
    private MilestonePaymentDate? _paymentDate = new("1", "7", "2026");

    public CompletionMilestoneDetailsBuilder WithMilestoneDateBeforeProgrammeStartDate()
    {
        _completionDate = new CompletionDate("1", "1", "2022");

        return this;
    }

    public CompletionMilestoneDetailsBuilder WithMissingMilestoneDate()
    {
        _completionDate = null;

        return this;
    }

    public CompletionMilestoneDetailsBuilder WithPaymentDateAfterProgrammeEndDate()
    {
        _paymentDate = new MilestonePaymentDate("1", "1", "2028");

        return this;
    }

    public CompletionMilestoneDetails Build()
    {
        return new CompletionMilestoneDetails(
            _completionDate,
            _paymentDate);
    }
}
