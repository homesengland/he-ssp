using System.Globalization;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class StartOnSiteMilestoneDetailsBuilder
{
    private StartOnSiteDate? _milestoneDate = new(true, "11", "1", "2025");
    private MilestonePaymentDate? _paymentDate = new(true, "1", "6", "2026");

    public StartOnSiteMilestoneDetailsBuilder WithAcquisitionDateBeforeProgrammeStartDate()
    {
        _milestoneDate = new StartOnSiteDate(true, "1", "1", "2022");

        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDateAfterProgrammeEndDate()
    {
        _paymentDate = new MilestonePaymentDate(true, "1", "1", "2028");

        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithStartOnSiteDate(DateOnly milestoneDate)
    {
        _milestoneDate = new StartOnSiteDate(
            true,
            milestoneDate.Day.ToString(CultureInfo.InvariantCulture), //todo ms unify all of these tostring invariantculture calls
            milestoneDate.Month.ToString(CultureInfo.InvariantCulture),
            milestoneDate.Year.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDate(DateOnly paymentDate)
    {
        return WithPaymentDate(new MilestonePaymentDate(
            true,
            paymentDate.Day.ToString(CultureInfo.InvariantCulture),
            paymentDate.Month.ToString(CultureInfo.InvariantCulture),
            paymentDate.Year.ToString(CultureInfo.InvariantCulture)));
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
