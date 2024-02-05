using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public class Section106TestDataBuilder
{
    private readonly string _siteId;

    private readonly string _siteName;

    private bool? _generalAgreement;

    private bool? _affordableHousing;

    private bool? _onlyAffordableHousing;

    private bool? _additionalAffordableHousing;

    private bool? _capitalFundingEligibility;

    private string? _localAuthorityConfirmation;

    public Section106TestDataBuilder()
    {
        _siteId = Guid.NewGuid().ToString();
        _siteName = "TestSite";
    }

    public Section106TestDataBuilder(string siteId, string siteName)
    {
        _siteId = siteId;
        _siteName = siteName;
    }

    private bool IsIneligible => IsIneligibleDueToCapitalFunding || IsIneligibleDueToAffordableHousing;

    private bool IsIneligibleDueToCapitalFunding => _capitalFundingEligibility == true;

    private bool IsIneligibleDueToAffordableHousing =>
        _affordableHousing == true
        && _onlyAffordableHousing == false
        && _additionalAffordableHousing == false;

    public Section106TestDataBuilder WithGeneralAgreement(bool? generalAgreement)
    {
        _generalAgreement = generalAgreement;
        return this;
    }

    public Section106TestDataBuilder WithAffordableHousing(bool? affordableHousing)
    {
        _affordableHousing = affordableHousing;
        return this;
    }

    public Section106TestDataBuilder WithOnlyAffordableHousing(bool? onlyAffordableHousing)
    {
        _onlyAffordableHousing = onlyAffordableHousing;
        return this;
    }

    public Section106TestDataBuilder WithAdditionalAffordableHousing(bool? additionalAffordableHousing)
    {
        _additionalAffordableHousing = additionalAffordableHousing;
        return this;
    }

    public Section106TestDataBuilder WithCapitalFundingEligibility(bool? capitalFundingEligibility)
    {
        _capitalFundingEligibility = capitalFundingEligibility;
        return this;
    }

    public Section106TestDataBuilder WithLocalAuthorityConfirmation(string? localAuthorityConfirmation)
    {
        _localAuthorityConfirmation = localAuthorityConfirmation;
        return this;
    }

    public Section106 Build()
    {
        return new Section106(
            SiteId: _siteId,
            SiteName: _siteName,
            GeneralAgreement: _generalAgreement,
            AffordableHousing: _affordableHousing,
            OnlyAffordableHousing: _onlyAffordableHousing,
            AdditionalAffordableHousing: _additionalAffordableHousing,
            CapitalFundingEligibility: _capitalFundingEligibility,
            LocalAuthorityConfirmation: _localAuthorityConfirmation,
            IsIneligible: IsIneligible,
            IsIneligibleDueToAffordableHousing: IsIneligibleDueToAffordableHousing,
            IsIneligibleDueToCapitalFunding: IsIneligibleDueToCapitalFunding);
    }
}
