using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsContributionsModel : FinancialDetailsBaseModel
{
    public FinancialDetailsContributionsModel()
        : base()
    {
    }

    public FinancialDetailsContributionsModel(
        Guid applicationId,
        string applicationName,
        string rentalIncomeBorrowing,
        string saleOfHomesOnThisScheme,
        string saleOfHomesOnOtherSchemes,
        string ownResources,
        string rCGFContribution,
        string otherCapitalSources,
        string initialSalesOfSharedHomes,
        string homesTransferValue,
        bool isSharedOwnership,
        bool isUnregisteredBodyAccount)
        : base(applicationId, applicationName)
    {
        RentalIncomeBorrowing = rentalIncomeBorrowing;
        SaleOfHomesOnThisScheme = saleOfHomesOnThisScheme;
        SaleOfHomesOnOtherSchemes = saleOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RCGFContribution = rCGFContribution;
        OtherCapitalSources = otherCapitalSources;
        InitialSalesOfSharedHomes = initialSalesOfSharedHomes;
        HomesTransferValue = homesTransferValue;
        IsSharedOwnership = isSharedOwnership; 
        IsUnregisteredBodyAccount = isUnregisteredBodyAccount;
    }

    public string RentalIncomeBorrowing { get; set; }

    public string SaleOfHomesOnThisScheme { get; set; }

    public string SaleOfHomesOnOtherSchemes { get; set; }

    public string OwnResources { get; set; }

    public string RCGFContribution { get; set; }

    public string OtherCapitalSources { get; set; }

    public string InitialSalesOfSharedHomes { get; set; }

    public string HomesTransferValue { get; set; }

    public bool IsSharedOwnership { get; set; }

    public bool IsUnregisteredBodyAccount { get; set; }

    public string TotalContributions
    {
        get
        {
            decimal result = 0;
            result += RentalIncomeBorrowing.TryParseNullableDecimal() ?? 0;
            result += SaleOfHomesOnThisScheme.TryParseNullableDecimal() ?? 0;
            result += SaleOfHomesOnOtherSchemes.TryParseNullableDecimal() ?? 0;
            result += OwnResources.TryParseNullableDecimal() ?? 0;
            result += RCGFContribution.TryParseNullableDecimal() ?? 0;
            result += OtherCapitalSources.TryParseNullableDecimal() ?? 0;
            result += InitialSalesOfSharedHomes.TryParseNullableDecimal() ?? 0;
            result += HomesTransferValue.TryParseNullableDecimal() ?? 0;

            return result.ToString(CultureInfo.InvariantCulture);
        }
    }
}
