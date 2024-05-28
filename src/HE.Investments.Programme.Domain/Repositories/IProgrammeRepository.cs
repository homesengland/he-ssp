using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Domain.Entities;

namespace HE.Investments.Programme.Domain.Repositories;

public interface IProgrammeRepository
{
    Task<ProgrammeEntity> GetProgramme(ProgrammeId programmeId, CancellationToken cancellationToken);

    Task<IList<ProgrammeEntity>> GetProgrammes(ProgrammeType programmeType, CancellationToken cancellationToken);
}
