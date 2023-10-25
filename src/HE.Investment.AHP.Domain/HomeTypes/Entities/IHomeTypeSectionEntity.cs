namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSectionEntity
{
    HomeTypeSectionType SectionType { get; }

    bool IsCompleted();
}
