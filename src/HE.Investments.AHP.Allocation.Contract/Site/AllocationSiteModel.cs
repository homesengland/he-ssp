using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.Allocation.Contract.Site;

public record AllocationSiteModel(AllocationId Id, string Name, Tenure Tenure, int NumberOfHomes);
