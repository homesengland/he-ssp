namespace HE.Investments.FrontDoor.Domain.Programme.Repository;

public interface IProgrammeRepository
{
    Task<bool> IsAnyAhpProgrammeAvailable(DateOnly? expectedStartDate, CancellationToken cancellationToken);
}
