using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class UploadFilesTests
{
    [Fact]
    public async Task ShouldThrowException_WhenThereIsAlreadyTenFilesUploaded()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<SupportingDocumentsParams>(10);
        var testCandidate = LoanApplicationTestBuilder.NewWithOtherApplicationStatus(ApplicationStatus.ReferredBackToApplicant).Build();

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            new[] { new SupportingDocumentsFile("test.pdf", 1000, 10, new MemoryStream()) },
            CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenFileWithTheSameNameIsAlreadyUploaded()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<SupportingDocumentsParams>(5);
        var testCandidate = LoanApplicationTestBuilder.NewWithOtherApplicationStatus(ApplicationStatus.ReferredBackToApplicant).Build();

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            new[] { new SupportingDocumentsFile("test-1.pdf", 1000, 10, new MemoryStream()) },
            CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldUploadFile_WhenFileNameIsUnique()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<SupportingDocumentsParams>(5);
        var testCandidate = LoanApplicationTestBuilder.NewWithOtherApplicationStatus(ApplicationStatus.ReferredBackToApplicant).Build();
        var file = new SupportingDocumentsFile("new-test.pdf", 1000, 10, new MemoryStream());

        // when
        var result = await testCandidate.UploadFiles(fileService, new[] { file }, CancellationToken.None);

        // then
        var uploadedFile = result.Should().HaveCount(1).And.Subject.Single();
        uploadedFile.Id.Should().NotBeNull();
    }
}
