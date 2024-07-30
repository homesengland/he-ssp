using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data;

public sealed record ClaimData(MilestoneType MilestoneType, DateDetails AchievementDate, bool Confirmation, bool? CostsIncurred = null);
