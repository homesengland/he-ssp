using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.CompanyStructureEntityTests;

public class UploadFilesTests
{
    [Fact]
    public async Task ShouldThrowException_WhenThereIsAlreadyTenFilesUploaded()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<LoanApplicationId>(10);
        var testCandidate = CompanyStructureEntityTestBuilder.New().Build();

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            new[] { new OrganisationMoreInformationFile("test.pdf", 1000, 10, new MemoryStream()) },
            CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenFileWithTheSameNameIsAlreadyUploaded()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<LoanApplicationId>(5);
        var testCandidate = CompanyStructureEntityTestBuilder.New().Build();

        // when
        var upload = () => testCandidate.UploadFiles(
            fileService,
            new[] { new OrganisationMoreInformationFile("test-1.pdf", 1000, 10, new MemoryStream()) },
            CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldUploadFile_WhenFileNameIsUnique()
    {
        // given
        var fileService = FileServiceMockTestBuilder.Build<LoanApplicationId>(5);
        var testCandidate = CompanyStructureEntityTestBuilder.New().WithStatus(SectionStatus.Completed).Build();
        var file = new OrganisationMoreInformationFile("new-test.pdf", 1000, 10, new MemoryStream());

        // when
        var result = await testCandidate.UploadFiles(fileService, new[] { file }, CancellationToken.None);

        // then
        var uploadedFile = result.Should().HaveCount(1).And.Subject.Single();
        uploadedFile.Id.Should().NotBeNull();
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }
}
