using System;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Tests.Helpers
{
    public static class HomeTypeHelper
    {
        public static invln_HomeType CreateHomeType(invln_scheme application, int numberOfHomesHomeType, decimal floorArea,
            invln_Typeofhousing typeOfHousing,
            invln_typeofolderpeopleshousing? typeOfOlderPeoplesHousing,
            invln_typeofhousingfordisabledvulnerable? typeofhousingfordisabledvulnerable)
        {
            return new invln_HomeType()
            {
                Id = Guid.NewGuid(),
                invln_application = application?.ToEntityReference(),
                invln_numberofhomeshometype = numberOfHomesHomeType,
                invln_floorarea = floorArea,
                invln_typeofhousing = new OptionSetValue((int)typeOfHousing),
                invln_typeofolderpeopleshousing = typeOfOlderPeoplesHousing != null ? new OptionSetValue((int)typeOfOlderPeoplesHousing.Value) : null,
                invln_typeofhousingfordisabledvulnerablepeople = typeofhousingfordisabledvulnerable != null ? new OptionSetValue((int)typeofhousingfordisabledvulnerable.Value) : null
            };
        }
    }
}
