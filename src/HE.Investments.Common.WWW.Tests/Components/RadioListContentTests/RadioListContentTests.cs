using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.TextAreaInput;
using HE.Investments.Common.WWW.Components.TextInput;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Common.WWW.Tests.Components.RadioListContentTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "ViewComponents tests are failing on CI from time to time.")]
public class RadioListContentTests : ViewComponentTestBase<RadioListContentTests>
{
    private const string ViewPath = "/Components/RadioListContentTests/RadioListContentTests.cshtml";

    private const string RadioInputFieldName = "fieldName";
    private const string TextInputFieldName = "secondNestedField";
    private const string TextAreaInputFieldName = "estedField";
    private readonly string[] _availableOptions = { "val1", "val2", "val3" };

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayView()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(ViewPath, model);

        // then
        document
            .HasRadio(RadioInputFieldName, _availableOptions, value: _availableOptions[1])
            .HasTextAreaInput(TextAreaInputFieldName)
            .HasInput(TextInputFieldName);
    }

    private RadioListContentViewModelTest CreateTestModel()
    {
        var items = new List<ExtendedSelectListItem>
        {
            new("item1", _availableOptions[0], false, "hint1", new DynamicComponentViewModel(nameof(TextAreaInput), new { fieldName = TextAreaInputFieldName })),
            new("item2", _availableOptions[1], true, "hint2", new DynamicComponentViewModel(nameof(TextInput), new { fieldName = TextInputFieldName })),
            new("item3", _availableOptions[2], false, "hint3"),
        };

        return new RadioListContentViewModelTest(RadioInputFieldName, items);
    }
}

public record RadioListContentViewModelTest(string FieldName, IEnumerable<ExtendedSelectListItem> AvailableOptions);
