using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.WWW.Workflows;

internal sealed class SiteSection106WorkflowConditions
{
    private readonly SiteModel? _siteModel;

    public SiteSection106WorkflowConditions(SiteModel? siteModel)
    {
        _siteModel = siteModel;
    }

    public bool IsGeneralAgreementAvailable => true;

    public bool IsAffordableHousingAvailable => _siteModel?.Section106?.GeneralAgreement == true;

    public bool IsOnlyAffordableHousingAvailable => IsAffordableHousingAvailable
                                                    && _siteModel?.Section106?.AffordableHousing == true;

    public bool IsAdditionalAffordableHousingAvailable => IsOnlyAffordableHousingAvailable
                                                          && _siteModel?.Section106?.OnlyAffordableHousing == false;

    public bool IsCapitalFundingEligibilityAvailable => IsAffordableHousingAvailable;

    public bool IsLocalAuthorityConfirmationAvailable => IsCapitalFundingEligibilityAvailable
                                                         && IsAdditionalAffordableHousingAvailable
                                                         && _siteModel?.Section106?.AdditionalAffordableHousing == true
                                                         && IsEligible;

    public bool IsEligible => !IsIneligible;

    public bool IsIneligible => _siteModel?.Section106?.IsIneligible == true;
}
