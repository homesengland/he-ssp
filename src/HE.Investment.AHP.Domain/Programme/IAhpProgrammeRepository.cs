using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Domain.Programme;

public interface IAhpProgrammeRepository
{
    Task<AhpProgramme> GetProgramme(AhpApplicationId applicationId, CancellationToken cancellationToken);
}
