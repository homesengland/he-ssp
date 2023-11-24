using HE.Investments.Loans.BusinessLogic.Projects.Enums;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
internal class PlanningPermissionStatusMapper
{
    public static PlanningPermissionStatus? Map(int? planningPermissionStatus)
    {
        return planningPermissionStatus switch
        {
            858110000 => PlanningPermissionStatus.NotSubmitted,
            858110001 => PlanningPermissionStatus.NotReceived,
            858110002 => PlanningPermissionStatus.OutlineOrConsent,
            858110003 => PlanningPermissionStatus.ReceivedFull,
            _ => null,
        };
    }

    public static int? Map(PlanningPermissionStatus? planningPermissionStatus)
    {
        return planningPermissionStatus switch
        {
            PlanningPermissionStatus.NotSubmitted => 858110000,
            PlanningPermissionStatus.NotReceived => 858110001,
            PlanningPermissionStatus.OutlineOrConsent => 858110002,
            PlanningPermissionStatus.ReceivedFull => 858110003,
            _ => null,
        };
    }

    public static string? MapToString(PlanningPermissionStatus? planningPermissionStatus)
    {
        return planningPermissionStatus switch
        {
            PlanningPermissionStatus.NotSubmitted => "notSubmitted",
            PlanningPermissionStatus.NotReceived => "notReceived",
            PlanningPermissionStatus.OutlineOrConsent => "outlineOrConsent",
            PlanningPermissionStatus.ReceivedFull => "receivedFull",
            _ => null,
        };
    }
}
