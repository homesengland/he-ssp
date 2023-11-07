namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSegmentEntity
{
    IHomeTypeSegmentEntity Duplicate();

    bool IsCompleted();
}
