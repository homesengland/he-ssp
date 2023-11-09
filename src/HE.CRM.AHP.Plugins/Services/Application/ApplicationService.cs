using System;
using System.Collections.Generic;
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

        public List<AhpApplicationDto> GetApplication(string organisationId, string contactId, string fieldsToRetrieve = null, string applicationId = null)
        {
            var listOfApplications = new List<AhpApplicationDto>();
            if (applicationId != null && Guid.TryParse(applicationId, out var applicationGuid))
            {
                var application = !string.IsNullOrEmpty(fieldsToRetrieve)
                    ? _applicationRepository.GetById(applicationGuid, new string[] { fieldsToRetrieve })
                    : _applicationRepository.GetById(applicationGuid);
                listOfApplications.Add(AhpApplicationMapper.MapRegularEntityToDto(application));
            }
            else
            {
                string attributes = null;
                if (!string.IsNullOrEmpty(fieldsToRetrieve))
                {
                    attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
                }
                var applications = _applicationRepository.GetApplicationsForOrganisationAndContact(organisationId, contactId, attributes);
                if (applications.Any())
                {
                    foreach (var application in applications)
                    {
                        listOfApplications.Add(AhpApplicationMapper.MapRegularEntityToDto(application));
                    }
                }

            }
            return listOfApplications;
        }

        public Guid SetApplication(string applicationSerialized, string organisationId, string contactId, string fieldsToUpdate = null)
        {
            var application = JsonSerializer.Deserialize<AhpApplicationDto>(applicationSerialized);
            var applicationMapped = AhpApplicationMapper.MapDtoToRegularEntity(application);
            invln_scheme applicationToUpdateOrCreate;
            if (!string.IsNullOrEmpty(fieldsToUpdate))
            {
                var fields = fieldsToUpdate?.Split(',');
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
                applicationToUpdateOrCreate.Id = new Guid(application.id);
                _applicationRepository.Update(applicationToUpdateOrCreate);
                return applicationToUpdateOrCreate.Id;
            }
        }

        private string GenerateFetchXmlAttributes(string fieldsToRetrieve)
        {
            var fields = fieldsToRetrieve.Split(',');
            var generatedAttribuesFetchXml = "";
            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    generatedAttribuesFetchXml += $"<attribute name=\"{field}\" />";
                }
            }
            return generatedAttribuesFetchXml;
        }

    }
}
