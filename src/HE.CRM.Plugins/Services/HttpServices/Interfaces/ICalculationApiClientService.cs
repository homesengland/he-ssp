using HE.AZURE.CalculationAPI.DTO;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.HttpService
{
    public interface ICalculationApiClientService : ICrmService
    {
        decimal? CalculateScores(string criteriaString, ScoringRangeDto[] scoringRange, string interimCalculationFormulas, string finalCalculationFormula);
    }
}
