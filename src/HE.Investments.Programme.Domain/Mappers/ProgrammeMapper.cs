using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Domain.Entities;
using HE.Investments.Programme.Domain.ValueObjects;

namespace HE.Investments.Programme.Domain.Mappers;

public static class ProgrammeMapper
{
    public static Contract.Programme Map(ProgrammeEntity entity)
    {
        return new Contract.Programme(
            entity.Id,
            entity.ShortName,
            entity.Name,
            entity.ProgrammeType,
            ToDateRange(entity.ProgrammeDates),
            ToDateRange(entity.FundingDates),
            ToDateRange(entity.StartOnSiteDates),
            ToDateRange(entity.CompletionDates));
    }

    private static DateRange ToDateRange(ProgrammeDates dates)
    {
        return new DateRange(dates.Start, dates.End);
    }
}
