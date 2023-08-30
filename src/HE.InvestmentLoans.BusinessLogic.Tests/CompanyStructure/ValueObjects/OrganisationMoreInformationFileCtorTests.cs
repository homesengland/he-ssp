using System.Globalization;
using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

[TestClass]
public class OrganisationMoreInformationFileCtorTests
{
    [TestMethod]
    [DataRow("document.pdf")]
    [DataRow("document.doc")]
    [DataRow("document.docx")]
    [DataRow("document.jpeg")]
    [DataRow("document.jpg")]
    [DataRow("document.rtf")]
    public void ShouldOrganisationMoreInformationFile(string fileName)
    {
        // given & when
        var organisationMoreInformationFile = new OrganisationMoreInformationFile(fileName, ByteArrayTestData.ByteArray1Kb, 1);

        // then
        organisationMoreInformationFile.FileName.Should().Be(fileName);
        organisationMoreInformationFile.Content.Should().NotBeEmpty();
    }

    [TestMethod]
    public void ShouldThrowDomainValidationException_WhenFileSizeIsToBig()
    {
        // given & when
        var action = () => new OrganisationMoreInformationFile("validFileName.doc", ByteArrayTestData.ByteArray1MbAnd1Kb, 1);

        // then
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .WithOnlyOneErrorMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, 1));
    }

    [TestMethod]
    public void ShouldThrowDomainValidationExceptionWithTwoError_WhenFileSizeIsToBigAndFileNameIsIncorrect()
    {
        // given & when
        var action = () => new OrganisationMoreInformationFile("validFileName.xxx", ByteArrayTestData.ByteArray1MbAnd1Kb, 1);

        // then
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .WithErrorMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, 1))
            .WithErrorMessage(ValidationErrorMessage.FileIncorrectFormat);
    }
}
