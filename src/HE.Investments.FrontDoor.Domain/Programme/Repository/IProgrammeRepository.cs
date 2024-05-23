namespace HE.Investments.FrontDoor.Domain.Programme.Repository;

public interface IProgrammeRepository
{
    Task<IEnumerable<ProgrammeDetails>> GetProgrammes(CancellationToken cancellationToken);
}
