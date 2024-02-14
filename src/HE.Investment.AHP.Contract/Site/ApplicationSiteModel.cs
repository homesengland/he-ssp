using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public record ApplicationSiteModel(AhpApplicationId Id, string Name, Tenure Tenure, int? NumberOfHomes, ApplicationStatus Status);
