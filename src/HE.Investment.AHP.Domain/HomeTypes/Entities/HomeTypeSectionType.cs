namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[Flags]
public enum HomeTypeSectionType
{
    Undefined = 0,
    HousingType = 1,
    HomeInformation = 2,
    DisabledAndVulnerablePeople = 4,
    OlderPeople = 8,
    All = HousingType | HomeInformation | DisabledAndVulnerablePeople | OlderPeople
}
