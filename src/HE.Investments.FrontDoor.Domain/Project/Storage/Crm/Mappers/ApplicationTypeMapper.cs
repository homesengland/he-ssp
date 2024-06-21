using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Crm.Mappers;

public class ApplicationTypeMapper : EnumMapper<ApplicationType>
{
    protected override IDictionary<ApplicationType, int?> Mapping => new Dictionary<ApplicationType, int?>
    {
        { ApplicationType.Loans, (int)he_FrontDoorDecision.Devfinance },
        { ApplicationType.Ahp, (int)he_FrontDoorDecision.AHP },
        { ApplicationType.Undefined, (int)he_FrontDoorDecision.Contactus },
    };
}
