using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSitePartnerDetailsQuery(string SiteId) : IRequest<SitePartnerDetailsModel>;
