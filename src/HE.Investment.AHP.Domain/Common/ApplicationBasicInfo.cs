using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;

namespace HE.Investment.AHP.Domain.Common;

public record ApplicationBasicInfo(Application.ValueObjects.ApplicationId Id, ApplicationName Name, Tenure Tenure, ApplicationStatus Status);
