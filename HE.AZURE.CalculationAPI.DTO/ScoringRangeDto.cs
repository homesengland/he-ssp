using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HE.AZURE.CalculationAPI.DTO
{
    [DataContract]
    public class ScoringRangeDto
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "value")]
        public string Value { get; set; }

        [Required]
        [DataMember(Name = "scoreName")]
        public string ScoreName { get; set; }

        [Required]
        [DataMember(Name = "score")]
        public int Score { get; set; }

        public ScoringRangeDto(string name, string value, string scoreName, int score)
        {
            Name = name;
            Value = value;
            ScoreName = scoreName;
            Score = score;
        }
    }
}
