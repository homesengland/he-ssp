using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFileCtorTests
{
    [Theory]
    [InlineData("document.pdf")]
    [InlineData("document.doc")]
    [InlineData("document.docx")]
    [InlineData("document.jpeg")]
    [InlineData("document.jpg")]
    [InlineData("document.rtf")]
    public void ShouldOrganisationMoreInformationFile(string fileName)
    {
        // given & when
        var organisationMoreInformationFile = new OrganisationMoreInformationFile(fileName, ByteArrayTestData.ByteArray1Kb, 1);

        // then
        organisationMoreInformationFile.FileName.Should().Be(fileName);
        organisationMoreInformationFile.Content.Should().NotBeEmpty();
    }

    [Fact]
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

    [Fact]
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
