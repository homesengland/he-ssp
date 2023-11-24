namespace HE.Investments.Loans.WWW.Models;

public class ConditionalModel
{
    public IEnumerable<(string RadioId, string ConditionalInputId)> Radios { get; set; }

    public IEnumerable<(string CheckboxId, string ConditionalInputId)> Checkboxes { get; set; }
}
