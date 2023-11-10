using HE.Investment.AHP.Domain.Application.Entities;

namespace HE.Investment.AHP.Domain.Common;

public enum ApplicationStatus
{
    Undefined = 0,
    Draft,
    Submitted,
}

public record ApplicationBasicInfo(Application.ValueObjects.ApplicationId Id, Tenure Tenure, ApplicationStatus Status);
