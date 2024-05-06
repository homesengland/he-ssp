namespace HE.Investment.AHP.Domain.Programme;

public interface IAhpProgrammeRepository
{
    Task<AhpProgramme> GetProgramme(CancellationToken cancellationToken);
}
