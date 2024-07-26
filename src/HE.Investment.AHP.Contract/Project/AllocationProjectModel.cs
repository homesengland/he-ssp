using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Project;

public record AllocationProjectModel(string Id, string Name, Tenure Tenure, int HouseToDeliver, string LocalAuthorityName);
