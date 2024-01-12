using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class DesignPlansSegmentContractMapper : IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans>
{
    public DesignPlans Map(ApplicationName applicationName, HomeTypeName homeTypeName, DesignPlansSegmentEntity segment)
    {
        return new DesignPlans(
            applicationName.Name,
            homeTypeName.Value,
            segment.DesignPrinciples.ToList(),
            segment.MoreInformation?.Value,
            segment.UploadedFiles.Select(x => MapDesignFile(x, segment.CanRemoveDesignFiles)).OrderBy(x => x.UploadedOn).ToList());
    }

    private static UploadedFile MapDesignFile(Common.UploadedFile file, bool canBeRemoved)
    {
        return new UploadedFile(file.Id, file.Name.Value, file.UploadedOn, file.UploadedBy, canBeRemoved);
    }
}
