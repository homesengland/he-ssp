using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
public static class OrganizationBasicInformationTestData
{
    public static readonly OrganizationBasicInformation OrganizationBasicInformationOne = new(
        "Test company",
        "112233",
        new("Aleje Jerozolimskie", "100", "12", "Warsaw", "123456", "Poland"));
}
