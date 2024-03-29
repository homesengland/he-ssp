using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.TestsUtils;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;

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
            ApplicationStatus.Draft,
            Mock.Of<IEventDispatcher>()));

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

    public CompanyStructureEntityTestBuilder WithStatus(SectionStatus status)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(_item, nameof(_item.Status), status);
        return this;
    }

    public CompanyStructureEntity Build()
    {
        return _item;
    }
}
