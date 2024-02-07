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

    public SiteWorkflowState CurrentState(SiteWorkflowState targetState)
    {
        if (targetState != SiteWorkflowState.Start || _siteModel == null)
        {
            return targetState;
        }

        return _siteModel switch
        {
            { Name: var x } when x.IsNotProvided() => SiteWorkflowState.Name,
            { Section106: var x } when x == null || x.GeneralAgreement.IsNotProvided() => SiteWorkflowState.Section106GeneralAgreement,
            { Section106: var x } when x!.AffordableHousing.IsNotProvided() && x.GeneralAgreement == true => SiteWorkflowState.Section106AffordableHousing,
            { Section106: var x } when x!.OnlyAffordableHousing.IsNotProvided() && x.GeneralAgreement == true => SiteWorkflowState.Section106OnlyAffordableHousing,
            { Section106: var x } when x!.AdditionalAffordableHousing.IsNotProvided() && x.OnlyAffordableHousing == false => SiteWorkflowState.Section106AdditionalAffordableHousing,
            { Section106: var x } when x!.CapitalFundingEligibility.IsNotProvided() && x.GeneralAgreement == true => SiteWorkflowState.Section106CapitalFundingEligibility,
            { Section106: var x } when x!.LocalAuthorityConfirmation.IsNotProvided() && x.AdditionalAffordableHousing == true => SiteWorkflowState.Section106LocalAuthorityConfirmation,
            { LocalAuthority: var x } when x.IsNotProvided() => SiteWorkflowState.LocalAuthoritySearch,
            { PlanningDetails: var x } when x.PlanningStatus.IsNotProvided() => SiteWorkflowState.PlanningStatus,
            { PlanningDetails.ArePlanningDetailsProvided: false } => SiteWorkflowState.PlanningDetails,
            { PlanningDetails: var x } when !IsLandRegistryProvided(x) => SiteWorkflowState.LandRegistry,
            { NationalDesignGuidePriorities: var x } when x.IsNotProvided() || x.Count == 0 => SiteWorkflowState.NationalDesignGuide,
            { BuildingForHealthyLife: BuildingForHealthyLifeType.Undefined } => SiteWorkflowState.BuildingForHealthyLife,
            { NumberOfGreenLights: var x } when x.IsNotProvided() && IsBuildingForHealthyLife() => SiteWorkflowState.NumberOfGreenLights,
            { TenderingStatusDetails: var x } when x.TenderingStatus.IsNotProvided() => SiteWorkflowState.TenderingStatus,
            { TenderingStatusDetails: var x } when IsConditionalOrUnconditionalWorksContract() &&
                                                   (x.ContractorName.IsNotProvided() || x.IsSmeContractor.IsNotProvided()) => SiteWorkflowState.ContractorDetails,
            { TenderingStatusDetails: var x } when IsTenderForWorksContractOrContractingHasNotYetBegun() &&
                                                   x.IsIntentionToWorkWithSme.IsNotProvided() => SiteWorkflowState.IntentionToWorkWithSme,
            _ => SiteWorkflowState.CheckAnswers,
        };
    }

    private bool CanBeAccessed(SiteWorkflowState state)
    {
        return state switch
        {
            SiteWorkflowState.Index => true,
            SiteWorkflowState.Start => true,
            SiteWorkflowState.Name => true,

            // TODO: #89874  add support for Section106 pages
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
            SiteWorkflowState.TenderingStatus => true,
            SiteWorkflowState.ContractorDetails => IsConditionalOrUnconditionalWorksContract(),
            SiteWorkflowState.IntentionToWorkWithSme => IsTenderForWorksContractOrContractingHasNotYetBegun(),
            SiteWorkflowState.StrategicSite => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SiteWorkflowState.Index)
            .Permit(Trigger.Continue, SiteWorkflowState.Start);

        _machine.Configure(SiteWorkflowState.Start)
            .Permit(Trigger.Continue, SiteWorkflowState.Name)
            .Permit(Trigger.Back, SiteWorkflowState.Index);

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
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106LocalAuthorityConfirmation, () => IsSection106EligibleWithAdditionalAffordableHousing())
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => IsSection106EligibleWithoutAdditionalAffordableHousing())
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
            .PermitIf(Trigger.Continue, SiteWorkflowState.TenderingStatus, () => !IsBuildingForHealthyLife())
            .Permit(Trigger.Back, SiteWorkflowState.NationalDesignGuide);

        _machine.Configure(SiteWorkflowState.NumberOfGreenLights)
            .Permit(Trigger.Continue, SiteWorkflowState.TenderingStatus)
            .Permit(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife);

        _machine.Configure(SiteWorkflowState.TenderingStatus)
            .PermitIf(Trigger.Continue, SiteWorkflowState.ContractorDetails, IsConditionalOrUnconditionalWorksContract)
            .PermitIf(Trigger.Continue, SiteWorkflowState.IntentionToWorkWithSme, IsTenderForWorksContractOrContractingHasNotYetBegun)
            .PermitIf(Trigger.Continue, SiteWorkflowState.CheckAnswers, IsNotApplicableOrMissing)
            .PermitIf(Trigger.Back, SiteWorkflowState.BuildingForHealthyLife, () => !IsBuildingForHealthyLife())
            .PermitIf(Trigger.Back, SiteWorkflowState.NumberOfGreenLights, IsBuildingForHealthyLife);

        _machine.Configure(SiteWorkflowState.ContractorDetails)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);

        _machine.Configure(SiteWorkflowState.IntentionToWorkWithSme)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SiteWorkflowState.TenderingStatus);
    }

    private bool IsLocalAuthorityProvided() => _siteModel?.LocalAuthority?.Name.IsProvided() ?? false;

    private bool IsLandTitleRegistered() => _siteModel?.PlanningDetails.IsLandRegistryTitleNumberRegistered ?? false;

    private bool IsLandRegistryProvided(SitePlanningDetails planningDetails) => IsLandTitleRegistered() &&
                                                                                planningDetails.LandRegistryTitleNumber.IsProvided() &&
                                                                                planningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber.IsProvided();

    private bool IsConditionalOrUnconditionalWorksContract() => _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.UnconditionalWorksContract or
        SiteTenderingStatus.ConditionalWorksContract;

    private bool IsTenderForWorksContractOrContractingHasNotYetBegun() => _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.TenderForWorksContract or
        SiteTenderingStatus.ContractingHasNotYetBegun;

    private bool IsSection106EligibleWithAdditionalAffordableHousing() => _siteModel?.Section106?.IsIneligible == false && _siteModel?.Section106?.AdditionalAffordableHousing == true;

    private bool IsSection106EligibleWithoutAdditionalAffordableHousing() => _siteModel?.Section106?.IsIneligible == false && _siteModel?.Section106?.AdditionalAffordableHousing == false;

    private bool IsNotApplicableOrMissing() => _siteModel?.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.NotApplicable or null;

    private bool IsBuildingForHealthyLife() => _siteModel?.BuildingForHealthyLife is BuildingForHealthyLifeType.Yes;
}
