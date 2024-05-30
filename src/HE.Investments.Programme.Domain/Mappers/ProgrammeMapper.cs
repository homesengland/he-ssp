using HE.Investments.Programme.Domain.Entities;

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
            entity.ProgrammeDates.Start,
            entity.ProgrammeDates.End,
            entity.StartOnSiteDates.Start,
            entity.StartOnSiteDates.End);
    }
}
