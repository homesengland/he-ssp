using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Site;

public record AllocationSiteModel(AllocationId Id, string Name, Tenure Tenure, int NumberOfHomes);
