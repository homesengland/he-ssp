using FluentAssertions;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Xunit;

namespace HE.DocumentService.Tests.SharePoint.Models;

public class TableResultTests
{
    [Fact]
    public void ShouldCreateTableResult()
    {
        // given
        var fileTableRow = new FileTableRow
        {
            Editor = "Edit",
            FileName = "FileName",
            FolderPath = "FolderPath",
            Id = 12344,
            Metadata = "Meta",
            Modified = new DateTime(2022, 10, 4, 1, 1, 1, DateTimeKind.Local),
            Size = 5,
        };

        var list = new List<FileTableRow> { fileTableRow };

        // when
        var tableResult = new TableResult<FileTableRow>(list, 1);

        // then
        tableResult.Items.Should().HaveCount(1);
        tableResult.Items[0].Should().Be(fileTableRow);
    }
}
