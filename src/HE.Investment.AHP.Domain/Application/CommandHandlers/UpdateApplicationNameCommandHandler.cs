using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class UpdateApplicationNameCommandHandler : UpdateApplicationCommandHandler<UpdateApplicationNameCommand>
{
    public UpdateApplicationNameCommandHandler(IApplicationRepository repository)
        : base(repository)
    {
    }

    protected override async Task Update(UpdateApplicationNameCommand request, ApplicationEntity application, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        if (name == application.Name)
        {
            return;
        }

        if (await Repository.IsExist(name, cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        application.ChangeName(new ApplicationName(request.Name));
    }
}
