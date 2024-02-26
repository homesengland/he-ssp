using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.CompanyStructureEntityTests;

public class UploadFilesTests
{
    [Fact]
    public async Task ShouldThrowException_WhenThereIsAlreadyTenFilesUploaded()
    {
        // given
        var fileService = MockFileService(10);
        var testCandidate = CompanyStructureEntityTestBuilder.New().Build();
        var file = new OrganisationMoreInformationFile("test.pdf", 1000, 10, new MemoryStream());

        // when
        var upload = () => testCandidate.UploadFiles(fileService, new[] { file }, CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenFileWithTheSameNameIsAlreadyUploaded()
    {
        // given
        var fileService = MockFileService(5);
        var testCandidate = CompanyStructureEntityTestBuilder.New().Build();
        var file = new OrganisationMoreInformationFile("test-1.pdf", 1000, 10, new MemoryStream());

        // when
        var upload = () => testCandidate.UploadFiles(fileService, new[] { file }, CancellationToken.None);

        // then
        await upload.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldUploadFile_WhenFileNameIsUnique()
    {
        // given
        var fileService = MockFileService(5);
        var testCandidate = CompanyStructureEntityTestBuilder.New().WithStatus(SectionStatus.Completed).Build();
        var file = new OrganisationMoreInformationFile("new-test.pdf", 1000, 10, new MemoryStream());

        // when
        var result = await testCandidate.UploadFiles(fileService, new[] { file }, CancellationToken.None);

        // then
        var uploadedFile = result.Should().HaveCount(1).And.Subject.Single();
        uploadedFile.Id.Should().NotBeNull();
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }

    private static ILoansFileService<LoanApplicationId> MockFileService(int uploadedFiles = 1)
    {
        var files = Enumerable.Range(0, uploadedFiles).Select(x => new UploadedFile(FileId.GenerateNew(), $"test-{x}.pdf", DateTime.Now, "John")).ToList();
        var mock = new Mock<ILoansFileService<LoanApplicationId>>();

        mock.Setup(x => x.GetFiles(It.IsAny<LoanApplicationId>(), It.IsAny<CancellationToken>())).ReturnsAsync(files);
        mock.Setup(x => x.UploadFile(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<LoanApplicationId>(), It.IsAny<CancellationToken>()))
            .Returns<string, Stream, LoanApplicationId, CancellationToken>((name, _, _, _) =>
                Task.FromResult(new UploadedFile(FileId.GenerateNew(), name, DateTime.Now, "John")));

        return mock.Object;
    }
}
