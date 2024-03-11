using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.EventHandlersTests;

public class CreateLoanApplicationFolderStructureEventHandlerTests : TestBase<CreateLoanApplicationFolderStructureEventHandler>
{
    [Fact]
    public async Task ShouldCreateFolders()
    {
        // given
        var applicationId = new LoanApplicationId(Guid.NewGuid());
        var documentService = CreateAndRegisterDependencyMock<IDocumentService>();
        RegisterDependency(MockFileApplicationRepository(applicationId, "007"));

        // when
        await TestCandidate.Handle(new LoanApplicationHasBeenStartedEvent(applicationId, "test"), CancellationToken.None);

        // then
        var expectedFolders = new List<string>
        {
            "007/external/more-information-about-organization",
            "007/internal/more-information-about-organization",
            "007/internal/cashflow",
        };
        documentService.Verify(x => x.CreateFoldersAsync(It.IsAny<string>(), expectedFolders, CancellationToken.None), Times.Once);
    }

    private static IFileApplicationRepository MockFileApplicationRepository(LoanApplicationId applicationId, string basePath)
    {
        var mock = new Mock<IFileApplicationRepository>();
        mock.Setup(x => x.GetBaseFilePath(applicationId, CancellationToken.None))
            .ReturnsAsync(basePath);

        return mock.Object;
    }
}
