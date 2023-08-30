using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestDataBuilders;

public class CompanyStructureEntityTestObjectBuilder
{
    private readonly CompanyStructureEntity _item;

    private CompanyStructureEntityTestObjectBuilder(CompanyStructureEntity companyStructureEntity)
    {
        _item = companyStructureEntity;
    }

    public static CompanyStructureEntityTestObjectBuilder New() =>
        new(new CompanyStructureEntity(LoanApplicationIdTestData.LoanApplicationIdOne, null, null, null, null, SectionStatus.NotStarted));

    public CompanyStructureEntityTestObjectBuilder WithHomesBuild()
    {
        _item.ProvideHowManyHomesBuilt(HomesBuiltTestData.HomesBuilt1);
        return this;
    }

    public CompanyStructureEntityTestObjectBuilder WithCompanyPurpose()
    {
        _item.ProvideCompanyPurpose(CompanyPurpose.New(true));
        return this;
    }

    public CompanyStructureEntityTestObjectBuilder WithMoreInformation()
    {
        _item.ProvideMoreInformation(OrganisationMoreInformationTestData.MoreInformationShort);
        return this;
    }

    public CompanyStructureEntity Build()
    {
        return _item;
    }
}
