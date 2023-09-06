using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
public class OrganizationBasicInformationTestBuilder
{
    private readonly OrganizationBasicInformation _item;

    private OrganizationBasicInformationTestBuilder(OrganizationBasicInformation organizationBasicInformation)
    {
        _item = organizationBasicInformation;
    }

    public static OrganizationBasicInformationTestBuilder New() =>
        new(new OrganizationBasicInformation(
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.RegisteredCompanyName,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.CompanyRegistrationNumber,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.Address));

    public OrganizationBasicInformation Build()
    {
        return _item;
    }
}
