using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Documents;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;

public static class FileServiceMockTestBuilder
{
    public static ILoansFileService<TParams> Build<TParams>(int uploadedFiles = 1)
    {
        var files = Enumerable.Range(0, uploadedFiles).Select(x => new UploadedFile(FileId.GenerateNew(), $"test-{x}.pdf", DateTime.Now, "John")).ToList();
        var mock = new Mock<ILoansFileService<TParams>>();

        mock.Setup(x => x.GetFiles(It.IsAny<TParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(files);
        mock.Setup(x => x.UploadFile(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<TParams>(), It.IsAny<CancellationToken>()))
            .Returns<string, Stream, TParams, CancellationToken>((name, _, _, _) =>
                Task.FromResult(new UploadedFile(FileId.GenerateNew(), name, DateTime.Now, "John")));

        return mock.Object;
    }
}
