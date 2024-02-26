using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class SiteWorkflow : IStateRouting<SiteWorkflowState>
{
    private readonly StateMachine<SiteWorkflowState, Trigger> _machine;

    private readonly SiteModel? _siteModel;

    public SiteWorkflow(SiteWorkflowState currentSiteWorkflowState, SiteModel? siteModel)
    {
        _machine = new StateMachine<SiteWorkflowState, Trigger>(currentSiteWorkflowState);
        _siteModel = siteModel;
        ConfigureTransitions();
    }

    public async Task<SiteWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(SiteWorkflowState nextState)
    {
        return Task.FromResult(CanBeAccessed(nextState));
    }

    private bool CanBeAccessed(SiteWorkflowState state)
    {
        return state switch
        {
            SiteWorkflowState.Start => true,
            SiteWorkflowState.Name => true,
            SiteWorkflowState.LocalAuthoritySearch => true,
            SiteWorkflowState.LocalAuthorityResult => true,
            SiteWorkflowState.LocalAuthorityConfirm => true,
            SiteWorkflowState.LocalAuthorityReset => true,
            SiteWorkflowState.PlanningStatus => true,
            SiteWorkflowState.PlanningDetails => true,
            SiteWorkflowState.Section106GeneralAgreement => true,
            SiteWorkflowState.Section106AffordableHousing => true,
            SiteWorkflowState.Section106AdditionalAffordableHousing => true,
            SiteWorkflowState.Section106OnlyAffordableHousing => true,
            SiteWorkflowState.Section106CapitalFundingEligibility => true,
            SiteWorkflowState.Section106LocalAuthorityConfirmation => true,
            SiteWorkflowState.Section106Ineligible => true,
            SiteWorkflowState.NationalDesignGuide => true,
            SiteWorkflowState.BuildingForHealthyLife => true,
            SiteWorkflowState.NumberOfGreenLights => IsBuildingForHealthyLife(),
            SiteWorkflowState.LandRegistry => IsLandTitleRegistered(),
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
            SiteWorkflowState.MmcInformation => IsUsingModernMethodsOfConstruction(),
            SiteWorkflowState.MmcCategories => IsUsingModernMethodsOfConstruction(),
            SiteWorkflowState.Mmc3DCategory => Is3DCategorySelected(),
            SiteWorkflowState.Mmc2DCategory => Is2DCategorySelected(),
            SiteWorkflowState.Procurements => true,
            SiteWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SiteWorkflowState.Name)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106GeneralAgreement)
            .Permit(Trigger.Back, SiteWorkflowState.Start);

        _machine.Configure(SiteWorkflowState.Section106GeneralAgreement)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AffordableHousing, () => _siteModel?.Section106?.GeneralAgreement == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => _siteModel?.Section106?.GeneralAgreement == false)
            .Permit(Trigger.Back, SiteWorkflowState.Name);

        _machine.Configure(SiteWorkflowState.Section106AffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106OnlyAffordableHousing, () => _siteModel?.Section106?.AffordableHousing == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => _siteModel?.Section106?.AffordableHousing == false)
            .Permit(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement);

        _machine.Configure(SiteWorkflowState.Section106OnlyAffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => _siteModel?.Section106?.OnlyAffordableHousing == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _siteModel?.Section106?.OnlyAffordableHousing == false)
            .Permit(Trigger.Back, SiteWorkflowState.Section106AffordableHousing);

        _machine.Configure(SiteWorkflowState.Section106AdditionalAffordableHousing)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility)
            .Permit(Trigger.Back, SiteWorkflowState.Section106AffordableHousing);

        _machine.Configure(SiteWorkflowState.Section106CapitalFundingEligibility)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106Ineligible, () => _siteModel?.Section106?.IsIneligible == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106LocalAuthorityConfirmation, IsSection106EligibleWithAdditionalAffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, IsSection106EligibleWithoutAdditionalAffordableHousing)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _siteModel?.Section106?.OnlyAffordableHousing == false)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106OnlyAffordableHousing, () => _siteModel?.Section106?.OnlyAffordableHousing == true)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AffordableHousing, () => _siteModel?.Section106?.OnlyAffordableHousing == null);

        _machine.Configure(SiteWorkflowState.Section106LocalAuthorityConfirmation)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        _machine.Configure(SiteWorkflowState.Section106Ineligible)
            .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        _machine.Configure(SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityResult)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement, () => _siteModel?.Section106?.GeneralAgreement == false)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                () => _siteModel?.Section106?.AdditionalAffordableHousing != false && _siteModel?.Section106?.GeneralAgreement != false)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                () => _siteModel?.Section106?.AdditionalAffordableHousing == false && _siteModel?.Section106?.GeneralAgreement != false);

        _machine.Configure(SiteWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.LocalAuthorityReset)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthorityConfirm, IsLocalAuthorityProvided)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch, () => !IsLocalAuthorityProvided());

        _machine.Configure(SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LandRegistry, IsLandTitleRegistered)
            .PermitIf(Trigger.Continue, SiteWorkflowState.NationalDesignGuide, () => !IsLandTitleRegistered())
            .Permit(Trigger.Back, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.LandRegistry)
            .Permit(Trigger.Continue, SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Back, SiteWorkflowState.PlanningDetails);

        _machine.Configure(SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Continue, SiteWorkflowState.BuildingForHealthyLife)
            .PermitIf(Trigger.Back, SiteWorkflowState.LandRegistry, IsLandTitleRegistered)
            .PermitIf(Trigger.Back, SiteWorkflowState.PlanningDetails, () => !IsLandTitleRegistered());

        _machine.Configure(SiteWorkflowState.BuildingForHealthyLife)
            .PermitIf(Trigger.Continue, SiteWorkflowState.NumberOfGreenLights, IsBuildingForHealthyLife)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LandAcquisitionStatus, () => !IsBuildingForHealthyLife())
            .Permit(Trigger.Back, SiteWorkflowState.NationalDesignGuide);

        _machine.Configure(SiteWorkflowState.NumberOfGreenLights)
            .Permit(Trigger.Continue, SiteWorkflowState.LandAcquisitionStatus)
            .Permit(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife);

        _machine.Configure(SiteWorkflowState.LandAcquisitionStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.TenderingStatus)
            .PermitIf(Trigger.Back, SiteWorkflowState.NumberOfGreenLights, IsBuildingForHealthyLife)
            .PermitIf(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife, () => !IsBuildingForHealthyLife());

        _machine.Configure(SiteWorkflowState.TenderingStatus)
            .PermitIf(Trigger.Continue, SiteWorkflowState.ContractorDetails, IsConditionalOrUnconditionalWorksContract)
            .PermitIf(Trigger.Continue, SiteWorkflowState.IntentionToWorkWithSme, IsTenderForWorksContractOrContractingHasNotYetBegun)
            .PermitIf(Trigger.Continue, SiteWorkflowState.StrategicSite, IsNotApplicableOrMissing)
            .Permit(Trigger.Back, SiteWorkflowState.LandAcquisitionStatus);

        _machine.Configure(SiteWorkflowState.ContractorDetails)
            .Permit(Trigger.Continue, SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);

        _machine.Configure(SiteWorkflowState.IntentionToWorkWithSme)
            .Permit(Trigger.Continue, SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);

        _machine.Configure(SiteWorkflowState.StrategicSite)
            .Permit(Trigger.Continue, SiteWorkflowState.SiteType)
            .PermitIf(Trigger.Back, SiteWorkflowState.TenderingStatus, IsNotApplicableOrMissing)
            .PermitIf(Trigger.Back, SiteWorkflowState.IntentionToWorkWithSme, IsTenderForWorksContractOrContractingHasNotYetBegun)
            .PermitIf(Trigger.Back, SiteWorkflowState.ContractorDetails, IsConditionalOrUnconditionalWorksContract);

        _machine.Configure(SiteWorkflowState.SiteType)
            .Permit(Trigger.Continue, SiteWorkflowState.SiteUse)
            .Permit(Trigger.Back, SiteWorkflowState.StrategicSite);

        _machine.Configure(SiteWorkflowState.SiteUse)
            .PermitIf(Trigger.Continue, SiteWorkflowState.TravellerPitchType, IsForTravellerPitchSite)
            .PermitIf(Trigger.Continue, SiteWorkflowState.RuralClassification, () => !IsForTravellerPitchSite())
            .Permit(Trigger.Back, SiteWorkflowState.SiteType);

        _machine.Configure(SiteWorkflowState.TravellerPitchType)
            .Permit(Trigger.Continue, SiteWorkflowState.RuralClassification)
            .PermitIf(Trigger.Back, SiteWorkflowState.SiteUse);

        _machine.Configure(SiteWorkflowState.RuralClassification)
            .Permit(Trigger.Continue, SiteWorkflowState.EnvironmentalImpact)
            .PermitIf(Trigger.Back, SiteWorkflowState.SiteUse, () => !IsForTravellerPitchSite())
            .PermitIf(Trigger.Back, SiteWorkflowState.TravellerPitchType, IsForTravellerPitchSite);

        _machine.Configure(SiteWorkflowState.EnvironmentalImpact)
            .Permit(Trigger.Continue, SiteWorkflowState.MmcUsing)
            .Permit(Trigger.Back, SiteWorkflowState.RuralClassification);

        _machine.Configure(SiteWorkflowState.MmcUsing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.MmcFutureAdoption, IsNotUsingModernMethodsOfConstruction)
            .PermitIf(Trigger.Continue, SiteWorkflowState.MmcInformation, IsUsingModernMethodsOfConstruction)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, IsUsingModernMethodsOfConstructionNotSelectedOrOnlyForSomeHomes)
            .Permit(Trigger.Back, SiteWorkflowState.EnvironmentalImpact);

        _machine.Configure(SiteWorkflowState.MmcFutureAdoption)
            .Permit(Trigger.Continue, SiteWorkflowState.Procurements)
            .Permit(Trigger.Back, SiteWorkflowState.MmcUsing);

        _machine.Configure(SiteWorkflowState.MmcInformation)
            .Permit(Trigger.Continue, SiteWorkflowState.MmcCategories)
            .Permit(Trigger.Back, SiteWorkflowState.MmcUsing);

        _machine.Configure(SiteWorkflowState.MmcCategories)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc3DCategory, Is3DCategorySelected)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc2DCategory, () => Is2DCategorySelected() && !Is3DCategorySelected())
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, () => !Is3DOr2DCategorySelected())
            .Permit(Trigger.Back, SiteWorkflowState.MmcInformation);

        _machine.Configure(SiteWorkflowState.Mmc3DCategory)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Mmc2DCategory, Is2DCategorySelected)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Procurements, () => !Is2DCategorySelected())
            .Permit(Trigger.Back, SiteWorkflowState.MmcCategories);

        _machine.Configure(SiteWorkflowState.Mmc2DCategory)
            .Permit(Trigger.Continue, SiteWorkflowState.Procurements)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc3DCategory, Is3DCategorySelected)
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcCategories, () => !Is3DCategorySelected());

        _machine.Configure(SiteWorkflowState.Procurements)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc2DCategory, Is2DCategorySelected)
            .PermitIf(Trigger.Back, SiteWorkflowState.Mmc3DCategory, () => Is3DCategorySelected() && !Is2DCategorySelected())
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcCategories, () => IsUsingModernMethodsOfConstruction() && !Is3DOr2DCategorySelected())
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcUsing, IsUsingModernMethodsOfConstructionNotSelectedOrOnlyForSomeHomes)
            .PermitIf(Trigger.Back, SiteWorkflowState.MmcFutureAdoption, IsNotUsingModernMethodsOfConstruction);

        _machine.Configure(SiteWorkflowState.CheckAnswers)
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

    private bool IsSection106EligibleWithAdditionalAffordableHousing() =>
        _siteModel?.Section106?.IsIneligible == false && _siteModel?.Section106?.AdditionalAffordableHousing == true;

    private bool IsSection106EligibleWithoutAdditionalAffordableHousing() =>
        _siteModel?.Section106?.IsIneligible == false && _siteModel?.Section106?.AdditionalAffordableHousing != true;

    private bool IsNotApplicableOrMissing() => _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.NotApplicable or null;

    private bool IsBuildingForHealthyLife() => _siteModel?.BuildingForHealthyLife is BuildingForHealthyLifeType.Yes;

    private bool IsForTravellerPitchSite() => _siteModel?.SiteUseDetails.IsForTravellerPitchSite == true;

    private bool IsUsingModernMethodsOfConstruction() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.Yes;

    private bool IsNotUsingModernMethodsOfConstruction() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.No;

    private bool IsUsingModernMethodsOfConstructionNotSelectedOrOnlyForSomeHomes() =>
        _siteModel?.ModernMethodsOfConstruction.UsingModernMethodsOfConstruction is null or SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes;

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
