using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationBasicModel(string? Id, string? Name, Tenure Tenure);
