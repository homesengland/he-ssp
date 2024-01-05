namespace HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;

public static class GetSingleLoanApplicationForAccountAndContactResponseTestData
{
    public const string EmptyResponse =
        /*lang=json,strict*/
        @"
            [
                {
                    ""siteDetailsList"": [],
                    ""LastModificationOn"": ""2023-08-30T12:51:43Z"",
                    ""loanApplicationId"": ""a860b821-6730-ee11-bdf3-002248c652b4""
                }
            ]
        ";

    public const string ResponseWithCompanyStructureFields =
        /*lang=json,strict*/
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

    public const string ResponseWithFundingFields =
        /*lang=json,strict*/
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
