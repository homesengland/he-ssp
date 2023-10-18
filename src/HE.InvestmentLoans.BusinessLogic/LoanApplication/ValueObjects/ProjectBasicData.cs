using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
public class ProjectBasicData : LoanApplicationSection
{
    public ProjectBasicData(SectionStatus status, ProjectId id, HomesBuilt homesBuilt)
        : base(status)
    {
        Id = id;
        HomesBuilt = homesBuilt;
    }

    public ProjectId Id { get; }

    public HomesBuilt HomesBuilt { get; }

    public static ProjectBasicData New(ProjectId id, HomesBuilt homesBuilt) => new(SectionStatus.NotStarted, id, homesBuilt);
}
