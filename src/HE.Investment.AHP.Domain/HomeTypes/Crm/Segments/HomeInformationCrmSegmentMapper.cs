using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class HomeInformationCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<HomeInformationSegmentEntity>
{
    public HomeInformationCrmSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_numberofhomeshometype),
            nameof(invln_HomeType.invln_numberofbedrooms),
            nameof(invln_HomeType.invln_maxoccupancy),
            nameof(invln_HomeType.invln_numberofstoreys),
            nameof(invln_HomeType.invln_homesusedformoveonaccommodation),
            nameof(invln_HomeType.invln_homesdesignedforuseofparticular),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.HomeInformation;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new HomeInformationSegmentEntity(
            dto.numberOfHomes.IsProvided() ? new NumberOfHomes(dto.numberOfHomes!.Value) : null,
            dto.numberOfBedrooms.IsProvided() ? new NumberOfBedrooms(dto.numberOfBedrooms!.Value) : null,
            dto.maxOccupancy.IsProvided() ? new MaximumOccupancy(dto.maxOccupancy!.Value) : null,
            dto.numberOfStoreys.IsProvided() ? new NumberOfStoreys(dto.numberOfStoreys!.Value) : null,
            MapYesNoAnswer(dto.isMoveOnAccommodation),
            MapPeopleGroupForSpecificDesignFeatures(dto.homesDesignedForUseOfParticularGroup));
    }

    protected override HomeInformationSegmentEntity GetSegment(HomeTypeEntity entity) => entity.HomeInformation;

    protected override void MapToDto(HomeTypeDto dto, HomeInformationSegmentEntity segment)
    {
        dto.numberOfHomes = segment.NumberOfHomes?.Value;
        dto.numberOfBedrooms = segment.NumberOfBedrooms?.Value;
        dto.maxOccupancy = segment.MaximumOccupancy?.Value;
        dto.numberOfStoreys = segment.NumberOfStoreys?.Value;
        dto.isMoveOnAccommodation = MapYesNoAnswer(segment.IntendedAsMoveOnAccommodation);
        dto.homesDesignedForUseOfParticularGroup = MapPeopleGroupForSpecificDesignFeatures(segment.PeopleGroupForSpecificDesignFeatures);
    }

    private static bool? MapYesNoAnswer(YesNoType yesNoAnswer)
    {
        return yesNoAnswer switch
        {
            YesNoType.Yes => true,
            YesNoType.No => false,
            _ => null,
        };
    }

    private static YesNoType MapYesNoAnswer(bool? yesNoAnswer)
    {
        return yesNoAnswer switch
        {
            true => YesNoType.Yes,
            false => YesNoType.No,
            null => YesNoType.Undefined,
        };
    }

    private static int? MapPeopleGroupForSpecificDesignFeatures(PeopleGroupForSpecificDesignFeaturesType peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            PeopleGroupForSpecificDesignFeaturesType.EthnicMinority => (int)invln_homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground,
            PeopleGroupForSpecificDesignFeaturesType.DisabledPeople => (int)invln_homesdesignedforuseofparticulargrou.Disabledpeople,
            PeopleGroupForSpecificDesignFeaturesType.FaithGroups => (int)invln_homesdesignedforuseofparticulargrou.Faithgroups,
            PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence => (int)invln_homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence,
            PeopleGroupForSpecificDesignFeaturesType.YoungPeople => (int)invln_homesdesignedforuseofparticulargrou.Youngpeople,
            PeopleGroupForSpecificDesignFeaturesType.OlderPeople => (int)invln_homesdesignedforuseofparticulargrou.Olderpeople,
            PeopleGroupForSpecificDesignFeaturesType.NoneOfThese => (int)invln_homesdesignedforuseofparticulargrou.Noneoftheabove,
            _ => null,
        };
    }

    private static PeopleGroupForSpecificDesignFeaturesType MapPeopleGroupForSpecificDesignFeatures(int? peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            (int)invln_homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground => PeopleGroupForSpecificDesignFeaturesType.EthnicMinority,
            (int)invln_homesdesignedforuseofparticulargrou.Disabledpeople => PeopleGroupForSpecificDesignFeaturesType.DisabledPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Faithgroups => PeopleGroupForSpecificDesignFeaturesType.FaithGroups,
            (int)invln_homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence => PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence,
            (int)invln_homesdesignedforuseofparticulargrou.Youngpeople => PeopleGroupForSpecificDesignFeaturesType.YoungPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Olderpeople => PeopleGroupForSpecificDesignFeaturesType.OlderPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Noneoftheabove => PeopleGroupForSpecificDesignFeaturesType.NoneOfThese,
            _ => PeopleGroupForSpecificDesignFeaturesType.Undefined,
        };
    }
}
