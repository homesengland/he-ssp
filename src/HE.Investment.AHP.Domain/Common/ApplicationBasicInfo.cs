using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Domain.Common;

public record ApplicationBasicInfo(Application.ValueObjects.ApplicationId Id, Tenure Tenure, ApplicationStatus Status);
