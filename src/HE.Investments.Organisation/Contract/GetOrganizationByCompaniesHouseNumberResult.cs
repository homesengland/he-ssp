using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investments.Organisation.Contract;
public record GetOrganizationByCompaniesHouseNumberResult(OrganisationSearchItem Item, string Error)
{
    public bool IsSuccessfull() => string.IsNullOrEmpty(Error);
}
