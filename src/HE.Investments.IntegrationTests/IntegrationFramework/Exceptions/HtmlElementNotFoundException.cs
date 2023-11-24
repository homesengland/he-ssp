namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework.Exceptions;
public class HtmlElementNotFoundException : Exception
{
    public HtmlElementNotFoundException(string? message)
        : base(message)
    {
    }
}
