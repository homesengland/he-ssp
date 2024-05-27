namespace HE.Investments.IntegrationTestsFramework.Exceptions;

public class ProblemWithTheServiceException : Exception
{
    public ProblemWithTheServiceException(string message)
        : base(message)
    {
    }
}
