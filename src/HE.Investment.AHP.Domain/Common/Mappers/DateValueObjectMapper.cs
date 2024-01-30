using System.Globalization;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Common.Mappers;

public static class DateValueObjectMapper
{
    public static DateDetails? ToContract(DateValueObject? date)
    {
        return date != null
            ? new DateDetails(
                date.Value.Day.ToString(CultureInfo.InvariantCulture),
                date.Value.Month.ToString(CultureInfo.InvariantCulture),
                date.Value.Year.ToString(CultureInfo.InvariantCulture))
            : null;
    }
}
