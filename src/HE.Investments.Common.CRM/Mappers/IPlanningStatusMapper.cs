using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.Common.CRM.Mappers;

public interface IPlanningStatusMapper
{
    int? ToDto(SitePlanningStatus? value);

    SitePlanningStatus? ToDomain(int? value);
}
