using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

[SuppressMessage("Performance", "CA1863", Justification = "Reviewed")]
public class OrganisationMoreInformationFileCtorTests
{
    [Theory]
    [InlineData("document.pdf")]
    [InlineData("document.doc")]
    [InlineData("document.docx")]
    [InlineData("document.jpeg")]
    [InlineData("document.jpg")]
    [InlineData("document.rtf")]
    public void ShouldCreateFile_WhenFileExtensionAndSizeIsValid(string fileName)
    {
        // given
        const long fileSize = 1024; // 1 kB

        // when
        using var organisationMoreInformationFile = new OrganisationMoreInformationFile(fileName, fileSize, 1, new MemoryStream());

        // then
        organisationMoreInformationFile.FileName.Should().Be(fileName);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenFileSizeIsToBig()
    {
        // given
        const long fileSize = (1024L * 1024L) + 1024L; // 1.1MB

        // when
        var action = () => new OrganisationMoreInformationFile("validFileName.doc", fileSize, 1, new MemoryStream());

        // then
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .WithOnlyOneErrorMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, 1));
    }

    [Fact]
    public void ShouldThrowDomainValidationExceptionWithTwoError_WhenFileSizeIsToBigAndFileNameIsIncorrect()
    {
        // given
        const long fileSize = (1024L * 1024L) + 1024L; // 1.1MB

        // when
        var action = () => new OrganisationMoreInformationFile("validFileName.xxx", fileSize, 1, new MemoryStream());

        // then
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .WithErrorMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, 1))
            .WithErrorMessage(ValidationErrorMessage.FileIncorrectFormat);
    }
}
