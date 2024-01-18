using System.Diagnostics.CodeAnalysis;

namespace HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;

public static class GetSingleLoanApplicationForAccountAndContactResponseTestData
{
    [SuppressMessage("Style", "JSON002:Probable JSON string detected", Justification = "Causes formatting error")]
    public const string EmptyResponse =
            @"
            [
                {
                    ""siteDetailsList"": [],
                    ""LastModificationOn"": ""2023-08-30T12:51:43Z"",
                    ""loanApplicationId"": ""a860b821-6730-ee11-bdf3-002248c652b4""
                }
            ]
        ";

    [SuppressMessage("Style", "JSON002:Probable JSON string detected", Justification = "Causes formatting error")]
    public const string ResponseWithCompanyStructureFields =
        @"
            [
                {
                    ""siteDetailsList"": [],
                    ""LastModificationOn"": ""2023-08-30T12:51:43Z"",
                    ""loanApplicationId"": ""a860b821-6730-ee11-bdf3-002248c652b4"",
                    ""companyPurpose"": true,
                    ""existingCompany"": ""Short description"",
                    ""companyExperience"": 5,
                    ""CompanyStructureAndExperienceCompletionStatus"": 858110001
                }
            ]
        ";

    [SuppressMessage("Style", "JSON002:Probable JSON string detected", Justification = "Causes formatting error")]
    public const string ResponseWithFundingFields =
        @"
            [
                {
                    ""siteDetailsList"": [],
                    ""LastModificationOn"": ""2023-08-30T12:51:43Z"",
                    ""loanApplicationId"": ""a860b821-6730-ee11-bdf3-002248c652b4"",
                    ""projectGdv"": 5.23,
                    ""projectEstimatedTotalCost"": 10,
                    ""projectAbnormalCosts"": true,
                    ""projectAbnormalCostsInformation"": ""Short description"",
                    ""privateSectorApproach"": true,
                    ""privateSectorApproachInformation"": ""Short description"",
                    ""additionalProjects"": true,
                    ""refinanceRepayment"": ""refinance"",
                    ""refinanceRepaymentDetails"": ""Short description"",
                    ""FundingDetailsCompletionStatus"": 858110001
                }
            ]
        ";
}
