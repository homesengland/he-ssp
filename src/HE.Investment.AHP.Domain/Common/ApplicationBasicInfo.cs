using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Common;

public record ApplicationBasicInfo(AhpApplicationId Id, ApplicationName Name, Tenure Tenure, ApplicationStatus Status);
