using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class UpdateApplicationNameCommandHandler : UpdateApplicationCommandHandler<UpdateApplicationNameCommand>
{
    public UpdateApplicationNameCommandHandler(IApplicationRepository repository)
        : base(repository)
    {
    }

    protected override void Update(UpdateApplicationNameCommand request, ApplicationEntity application)
    {
        application.ChangeName(new ApplicationName(request.Name));
    }
}
