using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public record ApplicationSiteModel(string Id, string Name, Tenure Tenure, ApplicationStatus Status);
