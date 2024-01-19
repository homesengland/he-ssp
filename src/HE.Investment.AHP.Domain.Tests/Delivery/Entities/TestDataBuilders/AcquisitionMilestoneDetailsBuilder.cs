using System.Globalization;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class AcquisitionMilestoneDetailsBuilder
{
    public static readonly AcquisitionDate? DefaultAcquisitionDate = new("10", "1", "2025");
    public static readonly MilestonePaymentDate? DefaultPaymentDate = new("1", "5", "2026");

    private AcquisitionDate? _acquisitionDate = DefaultAcquisitionDate;
    private MilestonePaymentDate? _paymentDate = DefaultPaymentDate;

    public AcquisitionMilestoneDetailsBuilder WithPaymentDateBeforeMilestoneDate()
    {
        _paymentDate = new MilestonePaymentDate("9", "1", "2025");

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithPaymentDateAtMilestoneDate()
    {
        _paymentDate = new MilestonePaymentDate("10", "1", "2025");

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithPaymentDate(MilestonePaymentDate paymentDate)
    {
        _paymentDate = paymentDate;

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithPaymentDate(DateOnly date)
    {
        _paymentDate = new MilestonePaymentDate(
            date.Day.ToString(CultureInfo.InvariantCulture),
            date.Month.ToString(CultureInfo.InvariantCulture),
            date.Year.ToString(CultureInfo.InvariantCulture));

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithoutPaymentDate()
    {
        _paymentDate = null;

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithAcquisitionDate(AcquisitionDate acquisitionDate)
    {
        _acquisitionDate = acquisitionDate;

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithAcquisitionDate(DateOnly date)
    {
        _acquisitionDate = new AcquisitionDate(
            date.Day.ToString(CultureInfo.InvariantCulture),
            date.Month.ToString(CultureInfo.InvariantCulture),
            date.Year.ToString(CultureInfo.InvariantCulture));

        return this;
    }

    public AcquisitionMilestoneDetailsBuilder WithoutAcquisitionDate()
    {
        _acquisitionDate = null;

        return this;
    }

    public AcquisitionMilestoneDetails Build()
    {
        return new AcquisitionMilestoneDetails(
            _acquisitionDate,
            _paymentDate);
    }
}
