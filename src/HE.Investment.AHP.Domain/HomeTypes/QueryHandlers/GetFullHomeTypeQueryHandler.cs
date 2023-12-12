using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetFullHomeTypeQueryHandler : IRequestHandler<GetFullHomeTypeQuery, FullHomeType>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> _designPlansSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> _disabledPeopleSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation> _homeInformationSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails> _olderPeopleSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation> _supportedHousingSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> _tenureDetailsSegmentMapper;

    public GetFullHomeTypeQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> designPlansSegmentMapper,
        IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> disabledPeopleSegmentMapper,
        IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation> homeInformationSegmentMapper,
        IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails> olderPeopleSegmentMapper,
        IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation> supportedHousingSegmentMapper,
        IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> tenureDetailsSegmentMapper)
    {
        _repository = repository;
        _designPlansSegmentMapper = designPlansSegmentMapper;
        _disabledPeopleSegmentMapper = disabledPeopleSegmentMapper;
        _homeInformationSegmentMapper = homeInformationSegmentMapper;
        _olderPeopleSegmentMapper = olderPeopleSegmentMapper;
        _supportedHousingSegmentMapper = supportedHousingSegmentMapper;
        _tenureDetailsSegmentMapper = tenureDetailsSegmentMapper;
    }

    public async Task<FullHomeType> Handle(GetFullHomeTypeQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            HomeTypeSegmentTypes.All,
            cancellationToken);

        return new FullHomeType(
            homeType.Id.Value,
            homeType.Name.Value,
            homeType.Application.Id.Value,
            homeType.Application.Name.Name,
            homeType.Application.Tenure,
            homeType.HousingType,
            homeType.Status == SectionStatus.Completed,
            MapOptionalSegment(homeType, homeType.OlderPeopleHomeTypeDetails, _olderPeopleSegmentMapper),
            MapOptionalSegment(homeType, homeType.DisabledPeopleHomeTypeDetails, _disabledPeopleSegmentMapper),
            MapOptionalSegment(homeType, homeType.DesignPlans, _designPlansSegmentMapper),
            MapOptionalSegment(homeType, homeType.SupportedHousingInformation, _supportedHousingSegmentMapper),
            MapOptionalSegment(homeType, homeType.HomeInformation, _homeInformationSegmentMapper)!,
            MapOptionalSegment(homeType, homeType.TenureDetails, _tenureDetailsSegmentMapper)!);
    }

    private static TSegmentContract? MapOptionalSegment<TSegmentEntity, TSegmentContract>(
        IHomeTypeEntity homeType,
        TSegmentEntity segment,
        IHomeTypeSegmentContractMapper<TSegmentEntity, TSegmentContract> segmentMapper)
        where TSegmentEntity : IHomeTypeSegmentEntity
        where TSegmentContract : HomeTypeSegmentBase
    {
        return segment.IsRequired(homeType.HousingType)
            ? MapRequiredSegment(homeType, segment, segmentMapper)
            : null;
    }

    private static TSegmentContract MapRequiredSegment<TSegmentEntity, TSegmentContract>(
        IHomeTypeEntity homeType,
        TSegmentEntity segment,
        IHomeTypeSegmentContractMapper<TSegmentEntity, TSegmentContract> segmentMapper)
        where TSegmentEntity : IHomeTypeSegmentEntity
        where TSegmentContract : HomeTypeSegmentBase
    {
        return segmentMapper.Map(homeType.Application.Name, homeType.Name, segment);
    }
}
