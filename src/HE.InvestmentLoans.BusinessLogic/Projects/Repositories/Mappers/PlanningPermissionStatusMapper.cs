using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
internal class PlanningPermissionStatusMapper
{
    public static PlanningPermissionStatus? Map(int? sectionStatus)
    {
        return sectionStatus switch
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
}
