namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectApplicationDto
{
    public string ApplicationId { get; set; }

    public string ApplicationName { get; set; }

    public int? ApplicationStatus { get; set; }

    public decimal? RequiredFunding { get; set; }

    public int? NoOfHomes { get; set; }

    public int? Tenure { get; set; }

    public DateTime? LastModificationDate { get; set; }
}
