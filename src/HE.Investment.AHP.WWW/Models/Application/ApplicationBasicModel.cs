using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationBasicModel(string ProjectId, string? Id, string? Name, Tenure Tenure);
