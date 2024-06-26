using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Workflows;

public class SiteWorkflow : EncodedStateRouting<SiteWorkflowState>
{
    private readonly SiteModel? _siteModel;

    private readonly SiteSection106WorkflowConditions _section106;

    public SiteWorkflow(SiteWorkflowState currentWorkflowState, SiteModel? siteModel, bool isLocked = false)
        : base(currentWorkflowState, isLocked)
    {
        _siteModel = siteModel;
        _section106 = new(_siteModel);
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(SiteWorkflowState state, bool? isReadOnlyMode = null)
    {
        if (isReadOnlyMode ?? _siteModel?.IsReadOnly == true)
        {
            return state == SiteWorkflowState.CheckAnswers;
        }

        var isStateEligible = BuildDeadEndConditions(state).All(isValid => isValid());
        return isStateEligible && state switch
        {
            SiteWorkflowState.Start => true,
            SiteWorkflowState.Name => true,
            SiteWorkflowState.Section106GeneralAgreement => _section106.IsGeneralAgreementAvailable,
            SiteWorkflowState.Section106AffordableHousing => _section106.IsAffordableHousingAvailable,
            SiteWorkflowState.Section106OnlyAffordableHousing => _section106.IsOnlyAffordableHousingAvailable,
            SiteWorkflowState.Section106AdditionalAffordableHousing => _section106.IsAdditionalAffordableHousingAvailable,
            SiteWorkflowState.Section106CapitalFundingEligibility => _section106.IsCapitalFundingEligibilityAvailable,
            SiteWorkflowState.Section106Ineligible => _section106.IsIneligible,
            SiteWorkflowState.Section106LocalAuthorityConfirmation => _section106.IsLocalAuthorityConfirmationAvailable,
            SiteWorkflowState.LocalAuthoritySearch => true,
            SiteWorkflowState.LocalAuthorityResult => true,
            SiteWorkflowState.LocalAuthorityConfirm => true,
            SiteWorkflowState.LocalAuthorityReset => true,
            SiteWorkflowState.PlanningStatus => true,
            SiteWorkflowState.PlanningDetails => true,
            SiteWorkflowState.LandRegistry => IsLandTitleRegistered(),
            SiteWorkflowState.NationalDesignGuide => true,
            SiteWorkflowState.BuildingForHealthyLife => true,
            SiteWorkflowState.NumberOfGreenLights => IsBuildingForHealthyLife(),
            SiteWorkflowState.StartSitePartnersFlow => true,
            SiteWorkflowState.FinishSitePartnersFlow => true,
            SiteWorkflowState.LandAcquisitionStatus => true,
            SiteWorkflowState.TenderingStatus => true,
            SiteWorkflowState.ContractorDetails => IsConditionalOrUnconditionalWorksContract(),
            SiteWorkflowState.IntentionToWorkWithSme => IsTenderForWorksContractOrContractingHasNotYetBegun(),
            SiteWorkflowState.StrategicSite => true,
            SiteWorkflowState.SiteType => true,
            SiteWorkflowState.SiteUse => true,
            SiteWorkflowState.TravellerPitchType => IsForTravellerPitchSite(),
            SiteWorkflowState.RuralClassification => true,
            SiteWorkflowState.EnvironmentalImpact => true,
            SiteWorkflowState.MmcUsing => true,
            SiteWorkflowState.MmcFutureAdoption => IsNotUsingModernMethodsOfConstruction(),
            SiteWorkflowState.MmcInformation => IsUsingModernMethodsOfConstruction() || IsPartiallyUsingModernMethodsOfConstruction(),
            SiteWorkflowState.MmcCategories => IsUsingModernMethodsOfConstruction(),
            SiteWorkflowState.Mmc3DCategory => Is3DCategorySelected(),
            SiteWorkflowState.Mmc2DCategory => Is2DCategorySelected(),
            SiteWorkflowState.Procurements => true,
            SiteWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private IEnumerable<Func<bool>> BuildDeadEndConditions(SiteWorkflowState state)
    {
        if (state > SiteWorkflowState.Section106Ineligible)
        {
            yield return () => !_section106.IsIneligible;
        }
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(SiteWorkflowState.Name)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106GeneralAgreement)
            .Permit(Trigger.Back, SiteWorkflowState.Start);

        Machine.Configure(SiteWorkflowState.Section106GeneralAgreement)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AffordableHousing, () => _section106.IsAffordableHousingAvailable)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => !_section106.IsAffordableHousingAvailable)
            .Permit(Trigger.Back, SiteWorkflowState.Start);

        Machine.Configure(SiteWorkflowState.Section106AffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106OnlyAffordableHousing, () => _section106.IsOnlyAffordableHousingAvailable)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => !_section106.IsOnlyAffordableHousingAvailable)
            .Permit(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement);

        Machine.Configure(SiteWorkflowState.Section106OnlyAffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => !_section106.IsAdditionalAffordableHousingAvailable)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _section106.IsAdditionalAffordableHousingAvailable)
            .Permit(Trigger.Back, SiteWorkflowState.Section106AffordableHousing);

        Machine.Configure(SiteWorkflowState.Section106AdditionalAffordableHousing)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility)
            .Permit(Trigger.Back, SiteWorkflowState.Section106OnlyAffordableHousing);

        Machine.Configure(SiteWorkflowState.Section106CapitalFundingEligibility)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106Ineligible, () => _section106.IsIneligible)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106LocalAuthorityConfirmation, () => _section106.IsLocalAuthorityConfirmationAvailable)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => _section106 is { IsEligible: true, IsLocalAuthorityConfirmationAvailable: false })
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _section106.IsAdditionalAffordableHousingAvailable)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106OnlyAffordableHousing, () => _section106 is { IsAdditionalAffordableHousingAvailable: false, IsOnlyAffordableHousingAvailable: true })
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AffordableHousing, () => _section106 is { IsAdditionalAffordableHousingAvailable: false, IsOnlyAffordableHousingAvailable: false });

        Machine.Configure(SiteWorkflowState.Section106LocalAuthorityConfirmation)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        Machine.Configure(SiteWorkflowState.Section106Ineligible)
            .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        Machine.Configure(SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityResult)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement, () => _siteModel?.Section106?.GeneralAgreement == false)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                () => _siteModel?.Section106?.AdditionalAffordableHousing == true)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                () => _siteModel?.Section106?.AdditionalAffordableHousing != true && _siteModel?.Section106?.GeneralAgreement == true);

        Machine.Configure(SiteWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        Machine.Configure(SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus);

        Machine.Configure(SiteWorkflowState.LocalAuthorityReset)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        Machine.Configure(SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthorityConfirm, IsLocalAuthorityProvided)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch, () => !IsLocalAuthorityProvided());

        Machine.Configure(SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LandRegistry, IsLandTitleRegistered)
            .PermitIf(Trigger.Continue, SiteWorkflowState.NationalDesignGuide, () => !IsLandTitleRegistered())
            .Permit(Trigger.Back, SiteWorkflowState.PlanningStatus);

        Machine.Configure(SiteWorkflowState.LandRegistry)
            .Permit(Trigger.Continue, SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Back, SiteWorkflowState.PlanningDetails);

        Machine.Configure(SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Continue, SiteWorkflowState.BuildingForHealthyLife)
            .PermitIf(Trigger.Back, SiteWorkflowState.LandRegistry, IsLandTitleRegistered)
            .PermitIf(Trigger.Back, SiteWorkflowState.PlanningDetails, () => !IsLandTitleRegistered());

        Machine.Configure(SiteWorkflowState.BuildingForHealthyLife)
            .PermitIf(Trigger.Continue, SiteWorkflowState.NumberOfGreenLights, IsBuildingForHealthyLife)
            .PermitIf(Trigger.Continue, SiteWorkflowState.StartSitePartnersFlow, () => !IsBuildingForHealthyLife())
            .Permit(Trigger.Back, SiteWorkflowState.NationalDesignGuide);

        Machine.Configure(SiteWorkflowState.NumberOfGreenLights)
            .Permit(Trigger.Continue, SiteWorkflowState.StartSitePartnersFlow)
            .Permit(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife);

        Machine.Configure(SiteWorkflowState.StartSitePartnersFlow)
            .PermitIf(Trigger.Back, SiteWorkflowState.NumberOfGreenLights, IsBuildingForHealthyLife)
            .PermitIf(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife, () => !IsBuildingForHealthyLife());

        Machine.Configure(SiteWorkflowState.FinishSitePartnersFlow)
            .Permit(Trigger.Continue, SiteWorkflowState.LandAcquisitionStatus);

        Machine.Configure(SiteWorkflowState.LandAcquisitionStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.TenderingStatus)
            .Permit(Trigger.Back, SiteWorkflowState.FinishSitePartnersFlow);

        Machine.Configure(SiteWorkflowState.TenderingStatus)
            .PermitIf(Trigger.Continue, SiteWorkflowState.ContractorDetails, IsConditionalOrUnconditionalWorksContract)
            .PermitIf(Trigger.Continue, SiteWorkflowState.IntentionToWorkWithSme, IsTenderForWorksContractOrContractingHasNotYetBegun)
            .PermitIf(Trigger.Continue, SiteWorkflowState.StrategicSite, IsNotApplicableOrMissing)
            .Permit(Trigger.Back, SiteWorkflowState.LandAcquisitionStatus);

        Machine.Configure(SiteWorkflowState.ContractorDetails)
            .Permit(Trigger.Continue, SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);

        Machine.Configure(SiteWorkflowState.IntentionToWorkWithSme)
            .Permit(Trigger.Continue, SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);

        Machine.Configure(SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Continue, SiteWorkflowState.SiteType)
            .PermitIf(Trigger.Back, SiteWorkflowState.TenderingStatus, IsNotApplicableOrMissing)
            .PermitIf(Trigger.Back, SiteWorkflowState.IntentionToWorkWithSme, IsTenderForWorksContractOrContractingHasNotYetBegun)
            .PermitIf(Trigger.Back, SiteWorkflowState.ContractorDetails, IsConditionalOrUnconditionalWorksContract);

        Machine.Configure(SiteWorkflowState.SiteType)
            .Permit(Trigger.Continue, SiteWorkflowState.SiteUse)
            .Permit(Trigger.Back, SiteWorkflowState.StrategicSite);

        Machine.Configure(SiteWorkflowState.SiteUse)
            .PermitIf(Trigger.Continue, SiteWorkflowState.TravellerPitchType, IsForTravellerPitchSite)
            .PermitIf(Trigger.Continue, SiteWorkflowState.RuralClassification, () => !IsForTravellerPitchSite())
            .Permit(Trigger.Back, SiteWorkflowState.SiteType);

        Machine.Configure(SiteWorkflowState.TravellerPitchType)
            .Permit(Trigger.Continue, SiteWorkflowState.RuralClassification)
            .PermitIf(Trigger.Back, SiteWorkflowState.SiteUse);

        Machine.Configure(SiteWorkflowState.RuralClassification)
            .Permit(Trigger.Continue, SiteWorkflowState.EnvironmentalImpact)
            .PermitIf(Trigger.Back, SiteWorkflowState.SiteUse, () => !IsForTravellerPitchSite())
            .PermitIf(Trigger.Back, SiteWorkflowState.TravellerPitchType, IsForTravellerPitchSite);

        Machine.Configure(SiteWorkflowState.EnvironmentalImpact)
            .Permit(Trigger.Continue, SiteWorkflowState.MmcUsing)
            .Permit(Trigger.Back, SiteWorkflowState.RuralClassification);

        Machine.Configure(SiteWorkflowState.MmcUsing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.MmcFutureAdoption, IsNotUsingModernMethodsOfConstruction)
            .PermitIf(Trigger.Continue, SiteWorkflowState.MmcInformation, () => IsUsingModernMethodsOfConstruction() || IsPartiallyUsingModernMethodsOfConstruction())
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, IsUsingModernMethodsOfConstructionNotSelected)
            .Permit(Trigger.Back, SiteWorkflowState.EnvironmentalImpact);

        Machine.Configure(SiteWorkflowState.MmcFutureAdoption)
            .Permit(Trigger.Continue, SiteWorkflowState.Procurements)
            .Permit(Trigger.Back, SiteWorkflowState.MmcUsing);

        Machine.Configure(SiteWorkflowState.MmcInformation)
            .PermitIf(Trigger.Continue, SiteWorkflowState.MmcCategories, IsUsingModernMethodsOfConstruction)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, IsPartiallyUsingModernMethodsOfConstruction)
            .Permit(Trigger.Back, SiteWorkflowState.MmcUsing);

        Machine.Configure(SiteWorkflowState.MmcCategories)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc3DCategory, Is3DCategorySelected)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc2DCategory, () => Is2DCategorySelected() && !Is3DCategorySelected())
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, () => !Is3DOr2DCategorySelected())
            .Permit(Trigger.Back, SiteWorkflowState.MmcInformation);

        Machine.Configure(SiteWorkflowState.Mmc3DCategory)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc2DCategory, Is2DCategorySelected)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, () => !Is2DCategorySelected())
            .Permit(Trigger.Back, SiteWorkflowState.MmcCategories);

        Machine.Configure(SiteWorkflowState.Mmc2DCategory)
            .Permit(Trigger.Continue, SiteWorkflowState.Procurements)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc3DCategory, Is3DCategorySelected)
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcCategories, () => !Is3DCategorySelected());

        Machine.Configure(SiteWorkflowState.Procurements)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc2DCategory, Is2DCategorySelected)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc3DCategory, () => Is3DCategorySelected() && !Is2DCategorySelected())
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcCategories, () => IsUsingModernMethodsOfConstruction() && !Is3DOr2DCategorySelected())
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcInformation, IsPartiallyUsingModernMethodsOfConstruction)
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcUsing, IsUsingModernMethodsOfConstructionNotSelected)
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcFutureAdoption, IsNotUsingModernMethodsOfConstruction);

        Machine.Configure(SiteWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SiteWorkflowState.Procurements);
    }

    private bool IsLocalAuthorityProvided() => _siteModel?.LocalAuthority?.Name.IsProvided() ?? false;

    private bool IsLandTitleRegistered() => _siteModel?.PlanningDetails.IsLandRegistryTitleNumberRegistered ?? false;

    private bool IsConditionalOrUnconditionalWorksContract() =>
        _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.UnconditionalWorksContract or
            SiteTenderingStatus.ConditionalWorksContract;

    private bool IsTenderForWorksContractOrContractingHasNotYetBegun() =>
        _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.TenderForWorksContract or
            SiteTenderingStatus.ContractingHasNotYetBegun;

    private bool IsNotApplicableOrMissing() => _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.NotApplicable or null;

    private bool IsBuildingForHealthyLife() => _siteModel?.BuildingForHealthyLife is BuildingForHealthyLifeType.Yes;

    private bool IsForTravellerPitchSite() => _siteModel?.SiteUseDetails.IsForTravellerPitchSite == true;

    private bool IsUsingModernMethodsOfConstruction() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.Yes;

    private bool IsPartiallyUsingModernMethodsOfConstruction() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes;

    private bool IsNotUsingModernMethodsOfConstruction() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.No;

    private bool IsUsingModernMethodsOfConstructionNotSelected() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is null;

    private bool Is3DCategorySelected()
    {
        var methods = _siteModel?.ModernMethodsOfConstruction.ModernMethodsConstructionCategories;
        if (methods == null)
        {
            return false;
        }

        return methods.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems);
    }

    private bool Is2DCategorySelected()
    {
        var methods = _siteModel?.ModernMethodsOfConstruction.ModernMethodsConstructionCategories;
        if (methods == null)
        {
            return false;
        }

        return methods.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems);
    }

    private bool Is3DOr2DCategorySelected()
    {
        var methods = _siteModel?.ModernMethodsOfConstruction.ModernMethodsConstructionCategories;
        if (methods == null)
        {
            return false;
        }

        return methods.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems) ||
               methods.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems);
    }
}
