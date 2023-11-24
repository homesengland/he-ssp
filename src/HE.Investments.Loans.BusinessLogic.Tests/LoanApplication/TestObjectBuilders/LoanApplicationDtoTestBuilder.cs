using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
internal sealed class LoanApplicationDtoTestBuilder
{
    private readonly LoanApplicationDto _dto;

    public LoanApplicationDtoTestBuilder()
    {
        _dto = new LoanApplicationDto
        {
            loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne.ToString(),
            siteDetailsList = new List<SiteDetailsDto>(),
        };
    }

    public static LoanApplicationDtoTestBuilder New() => new();

    public LoanApplicationDtoTestBuilder WithProject(SiteDetailsDtoTestBuilder builder)
    {
        var siteDetails = builder.Build();

        _dto.siteDetailsList.Add(siteDetails);

        return this;
    }

    public LoanApplicationDto Build() => _dto;
}
