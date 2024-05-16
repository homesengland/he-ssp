using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.dtomapping;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Services.AhpProject
{
    public class AhpProjectService : CrmService, IAhpProjectService
    {

        private readonly IContactRepository _contactRepository;
        private readonly IAhpProjectRepository _ahpProjectRepository;

        public AhpProjectService(CrmServiceArgs args) : base(args)
        {
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpProjectRepository = CrmRepositoriesFactory.Get<IAhpProjectRepository>();
        }

        public string CreateRecordFromPortal(string externalContactId, string organisationId, string heProjectId, string ahpProjectName, string consortiumId)
        {
            Contact createdByContact = null;
            if (!string.IsNullOrEmpty(externalContactId))
            {
                createdByContact = _contactRepository.GetContactViaExternalId(externalContactId);
            }

            var entity = AhpProjectMapper.ToEntity(createdByContact, organisationId, heProjectId, ahpProjectName, consortiumId);
            var id = _ahpProjectRepository.Create(entity);
            return id.ToString();
        }

    }
}
