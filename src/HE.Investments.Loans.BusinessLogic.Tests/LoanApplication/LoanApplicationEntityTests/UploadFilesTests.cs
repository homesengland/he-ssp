using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class UploadFilesTests : TestBase<LoanApplicationEntity>
{
    [Fact]
    public async Task ShouldThrowException_WhenThereIsAlreadyTenFilesUploaded()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<SupportingDocumentsParams>(10);
        var testCandidate = LoanApplicationTestBuilder.NewWithOtherApplicationStatus(ApplicationStatus.ReferredBackToApplicant).Build();
        var eventDispatcher = CreateAndRegisterDependencyMock<IEventDispatcher>().Object;

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            [new SupportingDocumentsFile("test.pdf", 1000, 10, new MemoryStream())],
            eventDispatcher,
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
        var eventDispatcher = CreateAndRegisterDependencyMock<IEventDispatcher>().Object;

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            [new SupportingDocumentsFile("test-1.pdf", 1000, 10, new MemoryStream())],
            eventDispatcher,
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
        using var file = new SupportingDocumentsFile("new-test.pdf", 1000, 10, new MemoryStream());
        var eventDispatcher = CreateAndRegisterDependencyMock<IEventDispatcher>().Object;

        // when
        var result = await testCandidate.UploadFiles(fileService, [file], eventDispatcher, CancellationToken.None);

        // then
        var uploadedFile = result.Should().HaveCount(1).And.Subject.Single();
        uploadedFile.Id.Should().NotBeNull();
    }
}
