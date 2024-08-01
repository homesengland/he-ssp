using HE.Investments.Common.Utils;

namespace HE.Investments.IntegrationTestsFramework.Utils;

public sealed class DateTimeManipulator : IDateTimeProvider
{
    private double _timeTravelOffsetInSeconds;

    public DateTime Now => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local).AddSeconds(_timeTravelOffsetInSeconds);

    public DateTime UtcNow => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc).AddSeconds(_timeTravelOffsetInSeconds);

    public void TravelInTimeTo(DateTime expectedDateTime)
    {
        _timeTravelOffsetInSeconds = (expectedDateTime - DateTime.Now).TotalSeconds;
    }

    public void ResetTimeTravel()
    {
        _timeTravelOffsetInSeconds = 0;
    }
}
