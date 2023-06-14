using System;
using System.Net.Http;
using System.Text;
using HE.AZURE.CalculationAPI.DTO;
using Microsoft.Xrm.Sdk;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.HttpService
{
    public class CalculationApiClientService : AzureAdHttpClientService, ICalculationApiClientService
    {
        private static class Endpoints
        {
            public static string Calculate = "/crm/calculationapi/Calculate";
        }

        public CalculationApiClientService(CrmServiceArgs args) : base(args)
        {
        }

        public decimal? CalculateScores(string criteriaString, ScoringRangeDto[] scoringRange, string interimCalculationFormulas, string finalCalculationFormula)
        {
            base.Logger.Trace($"CalculationApiCrmService.CalculateScores");
            base.Logger.Trace($"Deserialize");
            var criteria = Deserialize<CriteriaDto[]>(criteriaString);

            var calculateRequestDto = new CalculateRequestDto
            {
                Criteria = criteria,
                ScoringRangeValues = scoringRange,
                InterimCalculationFormula = interimCalculationFormulas,
                FinalCalculationFormula = finalCalculationFormula
            };

            base.Logger.Trace($"Serialize");
            string json = Serialize<CalculateRequestDto>(calculateRequestDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            base.Logger.Trace($"JSON: {json}");
            base.Logger.Trace($"Calling API: {CalculationApiUrl}, Method: {Endpoints.Calculate}");

            var response = PostAsync(CalculationApiUrl, Endpoints.Calculate, content).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultJson = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                base.Logger.Trace($"HTTP response success");
                    
                var result = Deserialize<CalculateResponseDto>(resultJson);

                if (result.CalculationResult.HasValue)
                    return Convert.ToDecimal(result.CalculationResult.Value);
            }
            else
            {
                HandleError(response, resultJson);
            }
            return null;
        }

        private void HandleError(HttpResponseMessage responseMessage, string resultJson)
        {
            var messageResponse = Deserialize<string>(resultJson);
            
            string errorMessage = "";
            if (responseMessage.StatusCode == (System.Net.HttpStatusCode)422
                || responseMessage.StatusCode == (System.Net.HttpStatusCode.BadRequest))
            {
                errorMessage = $"There is some issue with metric calculation. \nDetails: {messageResponse})";
                base.Logger.Error(errorMessage);
                throw new InvalidPluginExecutionException(OperationStatus.Failed, 0, errorMessage, PluginHttpStatusCode.BadRequest);
            }
            else
            {
                errorMessage = $"Calculation API: Response status code does not indicate success: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}))";
                errorMessage += $"\nDetails: {messageResponse ?? "(empty)"}";
                base.Logger.Error(errorMessage);
                throw new InvalidPluginExecutionException(OperationStatus.Failed, 0, errorMessage, PluginHttpStatusCode.InternalServerError);
            }
        }
    }
}
