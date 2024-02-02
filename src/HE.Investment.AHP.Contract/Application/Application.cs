using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Application;

public record Application(
    AhpApplicationId Id,
    string Name,
    Tenure Tenure,
    ApplicationStatus Status,
    string? ReferenceNumber,
    ModificationDetails? LastModificationDetails,
    IList<ApplicationSection> Sections,
    bool IsReadOnly);
