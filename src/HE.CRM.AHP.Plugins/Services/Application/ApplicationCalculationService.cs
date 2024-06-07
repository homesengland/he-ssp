using System;
using System.Linq;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public class CalculationReportData
    {
        public bool RtsoExamption { get; set; }

        public int NumberOfBedsits { get; set; }

        public bool IsContractExists { get; set; }
    }

    public class CalculateDataForApplicationReportService : CrmService, ICalculateDataForApplicationReportService
    {
        private readonly IHomeTypeRepository _homeTypeRepository;

        public CalculateDataForApplicationReportService(CrmServiceArgs args) : base(args)
        {
            _homeTypeRepository = CrmRepositoriesFactory.Get<IHomeTypeRepository>();
        }

        public CalculationReportData Calculate(Guid applicationId)
        {
            var ahpContractsRepository = CrmRepositoriesFactory.GetBase<invln_ahpcontract, DataverseContext>();

            var homeTypeColumns = new string[]
            {
                invln_HomeType.Fields.invln_rtsoexempt,
                invln_HomeType.Fields.invln_buildingtype,
                invln_HomeType.Fields.invln_numberofhomeshometype
            };
            var homeTypes = _homeTypeRepository.GetByAttribute(invln_HomeType.Fields.invln_application, applicationId, homeTypeColumns);

            var rtsoexamption = homeTypes
                .Where(x => x.invln_rtsoexempt.HasValue)
                .Any(x => x.invln_rtsoexempt.Value);

            var numberOfBedsits = homeTypes
                .Where(x => new OptionSetValue((int)invln_Buildingtype.Bedsit).Equals(x.invln_buildingtype))
                .Sum(x => x.invln_numberofhomeshometype);

            var ahpContracts = ahpContractsRepository
                .GetByAttribute(invln_ahpcontract.Fields.invln_AHPApplication, applicationId,
                    new string[] { invln_ahpcontract.Fields.invln_ahpcontractId });

            return new CalculationReportData
            {
                RtsoExamption = rtsoexamption,
                NumberOfBedsits = numberOfBedsits ?? 0,
                IsContractExists = ahpContracts.Any()
            };
        }
    }

    internal interface ICalculateDataForApplicationReportService : ICrmService
    {
        CalculationReportData Calculate(Guid applicationId);
    }
}
