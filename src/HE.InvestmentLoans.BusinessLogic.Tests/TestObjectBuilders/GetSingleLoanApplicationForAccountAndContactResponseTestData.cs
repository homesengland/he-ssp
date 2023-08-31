namespace HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;

public static class GetSingleLoanApplicationForAccountAndContactResponseTestData
{
    public const string EmptyResponse =
        """
            [
                {
                    "siteDetailsList": [],
                    "LastModificationOn": "2023-08-30T12:51:43Z",
                    "loanApplicationId": "a860b821-6730-ee11-bdf3-002248c652b4"
                }
            ]
        """;

    public const string ResponseWithCompanyStructureFields =
        """
            [
                {
                    "siteDetailsList": [],
                    "LastModificationOn": "2023-08-30T12:51:43Z",
                    "loanApplicationId": "a860b821-6730-ee11-bdf3-002248c652b4",
                    "companyPurpose": true,
                    "existingCompany": "Short description",
                    "companyExperience": 5,
                    "CompanyStructureAndExperienceCompletionStatus": 858110001
                }
            ]
        """;
}
