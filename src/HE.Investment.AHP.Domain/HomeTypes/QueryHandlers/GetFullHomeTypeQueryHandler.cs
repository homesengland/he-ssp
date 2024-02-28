using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetFullHomeTypeQueryHandler : IRequestHandler<GetFullHomeTypeQuery, FullHomeType>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> _designPlansSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> _disabledPeopleSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation> _homeInformationSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails> _olderPeopleSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation> _supportedHousingSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> _tenureDetailsSegmentMapper;

    private readonly IHomeTypeSegmentContractMapper<ModernMethodsConstructionSegmentEntity, ModernMethodsConstruction> _modernMethodsConstructionSegmentMapper;

    public GetFullHomeTypeQueryHandler(
        IHomeTypeRepository repository,
        IAccountUserContext accountUserContext,
        IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> designPlansSegmentMapper,
        IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> disabledPeopleSegmentMapper,
        IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation> homeInformationSegmentMapper,
        IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails> olderPeopleSegmentMapper,
        IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation> supportedHousingSegmentMapper,
        IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> tenureDetailsSegmentMapper,
        IHomeTypeSegmentContractMapper<ModernMethodsConstructionSegmentEntity, ModernMethodsConstruction> modernMethodsConstructionSegmentMapper)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _designPlansSegmentMapper = designPlansSegmentMapper;
        _disabledPeopleSegmentMapper = disabledPeopleSegmentMapper;
        _homeInformationSegmentMapper = homeInformationSegmentMapper;
        _olderPeopleSegmentMapper = olderPeopleSegmentMapper;
        _supportedHousingSegmentMapper = supportedHousingSegmentMapper;
        _tenureDetailsSegmentMapper = tenureDetailsSegmentMapper;
        _modernMethodsConstructionSegmentMapper = modernMethodsConstructionSegmentMapper;
    }

    public async Task<FullHomeType> Handle(GetFullHomeTypeQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _repository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            HomeTypeSegmentTypes.All,
            cancellationToken);

        return new FullHomeType(
            homeType.Id,
            homeType.Name.Value,
            homeType.Application.Id,
            homeType.Application.Name.Name,
            homeType.Application.Tenure,
            homeType.Application.IsLocked(),
            homeType.HousingType,
            homeType.Status == SectionStatus.Completed,
            homeType.IsReadOnly,
            MapOptionalSegment(homeType, homeType.OlderPeopleHomeTypeDetails, _olderPeopleSegmentMapper),
            MapOptionalSegment(homeType, homeType.DisabledPeopleHomeTypeDetails, _disabledPeopleSegmentMapper),
            MapOptionalSegment(homeType, homeType.DesignPlans, _designPlansSegmentMapper),
            MapOptionalSegment(homeType, homeType.SupportedHousingInformation, _supportedHousingSegmentMapper),
            MapRequiredSegment(homeType, homeType.HomeInformation, _homeInformationSegmentMapper),
            MapRequiredSegment(homeType, homeType.TenureDetails, _tenureDetailsSegmentMapper),
            MapRequiredSegment(homeType, homeType.ModernMethodsConstruction, _modernMethodsConstructionSegmentMapper));
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
