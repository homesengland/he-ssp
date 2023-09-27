using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;

namespace HE.InvestmentLoans.Common.Contract.Models;

public record NotificationModel(string Title, string Description, string? LinkDescription, NotificationType Type);
