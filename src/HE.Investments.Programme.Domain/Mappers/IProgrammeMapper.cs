using HE.Investments.Programme.Domain.Entities;

namespace HE.Investments.Programme.Domain.Mappers;

public interface IProgrammeMapper
{
    public Contract.Programme Map(ProgrammeEntity entity);
}
