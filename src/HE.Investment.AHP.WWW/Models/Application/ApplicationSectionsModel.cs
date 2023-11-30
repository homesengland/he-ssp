using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationSectionsModel(string ApplicationId, string SiteName, string Name, ApplicationStatus Status, string? ReferenceNumber, ModificationDetails? LastModificationDetails, IList<ApplicationSection> Sections);
