using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Common;

namespace HE.Investment.AHP.Domain.Application.Mappers;

public static class ApplicationBasicInfoMapper
{
    public static ApplicationDetails Map(ApplicationBasicInfo application)
    {
        return new ApplicationDetails(
            application.Id,
            application.Name.Value,
            application.Tenure,
            application.Status,
            application.AllowedOperations.ToList());
    }
}
