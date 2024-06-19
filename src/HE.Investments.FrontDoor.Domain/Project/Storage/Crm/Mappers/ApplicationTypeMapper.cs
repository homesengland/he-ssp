using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Crm.Mappers;

public class ApplicationTypeMapper : EnumMapper<ApplicationType>
{
    protected override IDictionary<ApplicationType, int?> Mapping => new Dictionary<ApplicationType, int?>
    {
        { ApplicationType.Loans, 134370000 }, // todo replace with he_FrontDoorDecision enum cast to int
        { ApplicationType.Ahp, 134370001 },
        { ApplicationType.Undefined, 134370002 },
    };
}
