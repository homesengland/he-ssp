extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.User;

public class LoanUserContext : ILoanUserContext
{
    private readonly IUserContext _userContext;

    private readonly ICacheService _cacheService;

    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _service;

    private readonly IList<Guid> _accountIds = new List<Guid>();

    private UserAccount? _selectedAccount;

    public LoanUserContext(IUserContext userContext, ICacheService cacheService, IOrganizationServiceAsync2 service, IContactService contactService)
    {
        _userContext = userContext;
        _cacheService = cacheService;
        _service = service;
        _contactService = contactService;
    }

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email ?? string.Empty;

    public IReadOnlyCollection<string> Roles { get; }

    public async Task<Guid> GetSelectedAccountId()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _selectedAccount!.AccountId;
    }

    public async Task<IList<Guid>> GetAllAccountIds()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _accountIds;
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _selectedAccount!;
    }

    public async void RefreshDetails()
    {
        var contactDto = await _contactService.RetrieveUserProfile(_service, UserGlobalId.ToString()) ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());

        var userDetails = UserDetailsMapper.MapContactDtoToUserDetails(contactDto);

        _cacheService.SetValue(UserGlobalId.ToString(), userDetails);
    }

    public async Task<bool> IsProfileCompleted()
    {
        var selectedUser = await GetSelectedAccount();
        var userDetails = await _cacheService.GetValueAsync(
                                                selectedUser.UserGlobalId.ToString(),
                                                async () =>
                                                {
                                                    var res = await _contactService.RetrieveUserProfile(_service, UserGlobalId.ToString())
                                                        ?? throw new NotFoundException(nameof(ContactDto), selectedUser.UserGlobalId.ToString());
                                                    return UserDetailsMapper.MapContactDtoToUserDetails(res);
                                                }) ?? throw new NotFoundException(nameof(UserDetails), selectedUser.UserGlobalId.ToString());

        return userDetails.IsProfileCompleted();
    }

    private async Task LoadUserAccount()
    {
        var userAccount = await _cacheService.GetValueAsync(
            $"{nameof(this.LoadUserAccount)}_{_userContext.UserGlobalId}",
            async () => await _contactService.GetContactRoles(_service, _userContext.Email, PortalConstants.PortalType, _userContext.UserGlobalId.ToString()))
            ?? throw new LoanUserAccountIsMissingException();

        var accounts = userAccount.contactRoles.OrderBy(x => x.accountId).ToList();

        _accountIds.AddRange(accounts.Select(x => x.accountId).ToList());

        var selectedAccount = accounts.FirstOrDefault() ?? throw new LoanUserAccountIsMissingException();

        _selectedAccount = new UserAccount(UserGlobalId, Email, selectedAccount.accountId, selectedAccount.accountName);
    }
}
