using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class StartOnSiteMilestoneDetailsBuilder
{
    private StartOnSiteDate? _milestoneDate = new("11", "1", "2025");
    private MilestonePaymentDate? _paymentDate = new("1", "6", "2026");

    public StartOnSiteMilestoneDetailsBuilder WithAcquisitionDateBeforeProgrammeStartDate()
    {
        _milestoneDate = new StartOnSiteDate("1", "1", "2022");

        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDateAfterProgrammeEndDate()
    {
        _paymentDate = new MilestonePaymentDate("1", "1", "2028");

        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithStartOnSiteDate(DateOnly milestoneDate)
    {
        _milestoneDate = StartOnSiteDate.Create(milestoneDate);
        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDate(DateOnly paymentDate)
    {
        return WithPaymentDate(MilestonePaymentDate.Create(paymentDate));
    }

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDate(MilestonePaymentDate paymentDate)
    {
        _paymentDate = paymentDate;

        return this;
    }

    public StartOnSiteMilestoneDetails Build()
    {
        return new StartOnSiteMilestoneDetails(
            _milestoneDate,
            _paymentDate);
    }
}
