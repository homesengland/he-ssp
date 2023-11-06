using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class UpdateApplicationTenureCommandHandler : UpdateApplicationCommandHandler<UpdateApplicationTenureCommand>
{
    public UpdateApplicationTenureCommandHandler(IApplicationRepository repository)
        : base(repository)
    {
    }

    protected override void Update(UpdateApplicationTenureCommand request, ApplicationEntity application)
    {
        application.ChangeTenure(new ApplicationTenure(request.Tenure));
    }
}
