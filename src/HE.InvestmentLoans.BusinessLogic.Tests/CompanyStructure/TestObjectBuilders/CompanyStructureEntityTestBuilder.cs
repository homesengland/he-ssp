using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Common.Domain;
using ApplicationStatus = HE.InvestmentLoans.Contract.Application.Enums.ApplicationStatus;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;

public class CompanyStructureEntityTestBuilder
{
    private readonly CompanyStructureEntity _item;

    private CompanyStructureEntityTestBuilder(CompanyStructureEntity companyStructureEntity)
    {
        _item = companyStructureEntity;
    }

    public static CompanyStructureEntityTestBuilder New() =>
        new(new CompanyStructureEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            null,
            null,
            null,
            SectionStatus.NotStarted,
            ApplicationStatus.Draft));

    public CompanyStructureEntityTestBuilder WithHomesBuild()
    {
        _item.ProvideHowManyHomesBuilt(HomesBuiltTestData.HomesBuilt1);
        return this;
    }

    public CompanyStructureEntityTestBuilder WithCompanyPurpose()
    {
        _item.ProvideCompanyPurpose(CompanyPurpose.New(true));
        return this;
    }

    public CompanyStructureEntityTestBuilder WithMoreInformation()
    {
        _item.ProvideMoreInformation(OrganisationMoreInformationTestData.MoreInformationShort);
        return this;
    }

    public CompanyStructureEntity Build()
    {
        return _item;
    }
}
