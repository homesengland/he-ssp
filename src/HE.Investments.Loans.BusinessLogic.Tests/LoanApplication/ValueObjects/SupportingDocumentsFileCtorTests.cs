using System.Globalization;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.ValueObjects;

public class SupportingDocumentsFileCtorTests
{
    [Theory]
    [InlineData("document.pdf")]
    [InlineData("document.doc")]
    [InlineData("document.docx")]
    [InlineData("document.jpeg")]
    [InlineData("document.jpg")]
    [InlineData("document.rtf")]
    [InlineData("document.test")]
    [InlineData("document.zip")]
    [InlineData("document.vsx")]
    public void ShouldCreateFile_WhenFileExtensionAndSizeIsValid(string fileName)
    {
        // given
        const long fileSize = 1024; // 1 kB

        // when
        using var supportingDocumentsFile = new SupportingDocumentsFile(fileName, fileSize, 1, new MemoryStream());

        // then
        supportingDocumentsFile.FileName.Should().Be(fileName);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenFileSizeIsToBig()
    {
        // given
        const long fileSize = (1024L * 1024L) + 1024L; // 1.1MB

        // when
        var action = () => new SupportingDocumentsFile("validFileName.doc", fileSize, 1, new MemoryStream());

        // then
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .WithOnlyOneErrorMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, 1));
    }
}
