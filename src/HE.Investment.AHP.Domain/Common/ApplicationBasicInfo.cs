using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Common;

public record ApplicationBasicInfo(AhpApplicationId Id, SiteId SiteId, ApplicationName Name, Tenure Tenure, ApplicationStatus Status, AhpProgramme Programme)
{
    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(Status);
    }

    public bool IsLocked()
    {
        var lockedStatuses = ApplicationStatusDivision.GetAllStatusesForLockedMode();
        return lockedStatuses.Contains(Status);
    }
}
