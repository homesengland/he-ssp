namespace HE.InvestmentLoans.Common.Models.Others;

public class ErrorModel
{
    public ErrorModel()
    {
    }

    public ErrorModel(string message)
    {
        Message = message;
    }

    public ErrorModel(string message, string stackTrace)
    {
        Message = $"{message} {stackTrace}";
    }

    public string Message { get; set; }
}
