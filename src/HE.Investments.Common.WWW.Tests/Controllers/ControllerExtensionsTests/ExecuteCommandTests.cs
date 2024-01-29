using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.WWW.Attributes;
using HE.Investments.Common.WWW.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HE.Investments.Common.WWW.Tests.Controllers.ControllerExtensionsTests;

[SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "tests")]
public class ExecuteCommandTests
{
    private const string FirstKey = "One";
    private const string SecondKey = "Two";
    private readonly TestController _controller = new();
    private readonly TestCommand _command = new();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private readonly OperationResult _errorResult = OperationResult.New()
        .AddValidationError(FirstKey, "err1")
        .AddValidationError(SecondKey, "err2");

    private readonly Mock<IMediator> _mediatorMock = new();

    public ExecuteCommandTests()
    {
        _mediatorMock.Setup(i => i.Send(_command, _cancellationToken)).ReturnsAsync(_errorResult);
    }

    [Fact]
    public async Task ShouldOrderErrors_WhenModelIsAClass()
    {
        // given & when
        await Execute<TestModelClass>();

        // then
        AssertValidationErrors(FirstKey, SecondKey);
    }

    [Fact]
    public async Task ShouldOrderErrors_WhenModelIsARecord()
    {
        // given & when
        await Execute<TestModelRecord>();

        // then
        AssertValidationErrors(FirstKey, SecondKey);
    }

    [Fact]
    public async Task ShouldOrderErrors_WhenModelIsAClassWithCustomOrder()
    {
        // given & when
        await Execute<TestModelClassWithCustomOrder>();

        // then
        AssertValidationErrors(SecondKey, FirstKey);
    }

    [Fact]
    public async Task ShouldOrderErrors_WhenModelIsARecordWithCustomOrder()
    {
        // given & when
        await Execute<TestModelRecordWithCustomOrder>();

        // then
        AssertValidationErrors(SecondKey, FirstKey);
    }

    private async Task Execute<T>()
    {
        await _controller.ExecuteCommand<T>(
            _mediatorMock.Object,
            _command,
            async () => await Task.FromResult(new OkResult()),
            async () => await Task.FromResult(new OkResult()),
            _cancellationToken);
    }

    private void AssertValidationErrors(string firstKey, string secondKey)
    {
        var errors = _controller.ViewBag.validationErrors as Dictionary<string, string>;
        errors.Should().NotBeNull();
        errors!.Count.Should().Be(2);
        errors.First().Key.Should().Contain(firstKey);
        errors.Last().Key.Should().Contain(secondKey);
    }

    private class TestController : Controller
    {
    }

    private class TestCommand : IRequest<OperationResult>
    {
    }

    private class TestModelClass
    {
        public int One { get; set; }

        public IDictionary<string, int> Two { get; set; }
    }

    private class TestModelClassWithCustomOrder
    {
        [ErrorSummaryOrder(2)]
        public int One { get; set; }

        [ErrorSummaryOrder(1)]
        public IDictionary<string, int> Two { get; set; }
    }

    private record TestModelRecord(int One, IDictionary<string, int> Two);

    private record TestModelRecordWithCustomOrder([ErrorSummaryOrder(2)] int One, [ErrorSummaryOrder(1)] IDictionary<string, int> Two);
}
