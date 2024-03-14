using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.Programme
{
    public class ProgrammeDuplicateDetectionHandler : CrmEntityHandlerBase<invln_programme, DataverseContext>
    {
        private readonly IProgrammeRepository _programmeRepository;

        public ProgrammeDuplicateDetectionHandler(IProgrammeRepository programmeRepository)
        {
            _programmeRepository = programmeRepository;
        }

        public override bool CanWork()
        {
            return CurrentState.invln_programmename != null;
        }

        public override void DoWork()
        {
            var programmes = _programmeRepository.GetProgrammes(new string[] { invln_programme.Fields.invln_programmename });
            var isduplicatExist = programmes.Where(x => x.invln_programmename.ToLower() == CurrentState.invln_programmename.ToLower()).ToList();
            if (isduplicatExist.Any())
            {
                throw new InvalidPluginExecutionException($"There is already a programme with this name. Enter a different name");
            }
        }
    }
}
