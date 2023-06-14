using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HE.AZURE.CalculationAPI.DTO
{
    [DataContract]
    public class CalculateRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "The field Criteria must be an array type with a minimum length of '1'")]
        [DataMember(Name = "criteria")]
        public CriteriaDto[] Criteria { get; set; }

        [DataMember(Name = "scoringRangeValues")]
        public ScoringRangeDto[] ScoringRangeValues { get; set; }

        [DataMember(Name = "interimCalculationFormula")]
        public string InterimCalculationFormula { get; set; }

        [Required]
        [DataMember(Name = "finalCalculationFormula")]
        public string FinalCalculationFormula { get; set; }
    }
}
