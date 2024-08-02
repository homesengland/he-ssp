using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;

namespace HE.Investments.AHP.Allocation.Domain.Site.ValueObjects;

public record AllocationSiteDetails(AllocationId Id, string Name, Tenure Tenure, int NumberOfHomes);
