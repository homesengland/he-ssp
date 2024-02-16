using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Services.Programme
{
    public class ProgrammeService : CrmService, IProgrammeService
    {
        private readonly IProgrammeRepository _programmRepository;
        private readonly IMilestoneFrameworkItemRepository _milestoneFrameworkItemRepository;

        public ProgrammeService(CrmServiceArgs args) : base(args)
        {
            _programmRepository = CrmRepositoriesFactory.Get<IProgrammeRepository>();
            _milestoneFrameworkItemRepository = CrmRepositoriesFactory.Get<IMilestoneFrameworkItemRepository>();
        }

        public ProgrammeDto GetProgramme(string programmeId)
        {
            ProgrammeDto programmeDto = null;

            var programme = _programmRepository.GetProgrammeById(programmeId);
            if (programme != null)
            {
                var milestoneFrameworkItemRepository = _milestoneFrameworkItemRepository.GetMilestoneFrameworkItemByProgrammeId(programmeId);
                var milestoneFrameworkItemDtoList = MilestoneFrameWorkItemMapper.MapMilestoneFrameworkItemListToDto(milestoneFrameworkItemRepository);
                programmeDto = ProgrammeMapper.MapRegularEntityToDto(programme, milestoneFrameworkItemDtoList);
            }
            return programmeDto;
        }


        public List<ProgrammeDto> GetProgrammes()
        {
            var programmesDto = new List<ProgrammeDto>();

            var programmes = _programmRepository.GetProgrammes();
            if (programmes != null)
            {
                foreach (var programme in programmes)
                {
                    programmesDto.Add(ProgrammeMapper.MapRegularEntityToDto(programme));
                }
            }
            return programmesDto;
        }
    }
}
