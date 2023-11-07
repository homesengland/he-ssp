using System;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public class ApplicationService : CrmService, IApplicationService
    {
        private readonly IAhpApplicationRepository _applicationRepository;
        public ApplicationService(CrmServiceArgs args) : base(args)
        {
            _applicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
        }

        public AhpApplicationDto GetApplication(string applicationId)
        {
            throw new NotImplementedException();
        }

        public Guid SetApplication(string applicationSerialized, string fieldsToUpdate)
        {
            var application = JsonSerializer.Deserialize<AhpApplicationDto>(applicationSerialized);
            var applicationMapped = AhpApplicationMapper.MapDtoToRegularEntity(application);
            invln_scheme applicationToUpdateOrCreate;
            var fields = fieldsToUpdate.Split(',');
            if (fields.Length > 0)
            {
                applicationToUpdateOrCreate = new invln_scheme();
                foreach (var field in fields)
                {
                    TracingService.Trace($"field name {field}");
                    applicationToUpdateOrCreate[field] = applicationMapped[field];
                }
            }
            else
            {
                applicationToUpdateOrCreate = applicationMapped;
            }

            if (string.IsNullOrEmpty(application.id))
            {
                return _applicationRepository.Create(applicationToUpdateOrCreate);
            }
            else
            {
                _applicationRepository.Update(applicationToUpdateOrCreate);
                return applicationToUpdateOrCreate.Id;
            }
        }
    }
}
