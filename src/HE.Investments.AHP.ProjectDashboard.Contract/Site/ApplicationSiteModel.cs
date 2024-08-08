using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Site;

public record ApplicationSiteModel(AhpApplicationId Id, string Name, Tenure? Tenure, int? NumberOfHomes, ApplicationStatus Status);
