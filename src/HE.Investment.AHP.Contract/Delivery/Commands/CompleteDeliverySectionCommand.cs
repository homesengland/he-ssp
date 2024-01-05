using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CompleteDeliverySectionCommand(string ApplicationId, IsSectionCompleted IsSectionCompleted, bool IsCheckOnly = false) : IDeliveryCommand;
