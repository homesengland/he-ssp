FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base

RUN groupadd -g 999 appuser && \
 useradd -r -u 999 -g appuser appuser
USER appuser

WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:8.0.204 AS build

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["HE.Common.IntegrationModel.Crm/HE.Common.IntegrationModel.Crm.csproj", "HE.Common.IntegrationModel.Crm/"]
COPY ["HE.Investment.AHP.WWW/HE.Investment.AHP.WWW.csproj", "HE.Investment.AHP.WWW/"]
COPY ["HE.Investment.AHP.Contract/HE.Investment.AHP.Contract.csproj", "HE.Investment.AHP.Contract/"]
COPY ["HE.Investment.AHP.Domain/HE.Investment.AHP.Domain.csproj", "HE.Investment.AHP.Domain/"]
COPY ["HE.Investments.Account.Contract/HE.Investments.Account.Contract.csproj", "HE.Investments.Account.Contract/"]
COPY ["HE.Investments.Account.Shared/HE.Investments.Account.Shared.csproj", "HE.Investments.Account.Shared/"]
COPY ["HE.Investments.AHP.Allocation.Contract/HE.Investments.AHP.Allocation.Contract.csproj", "HE.Investments.AHP.Allocation.Contract/"]
COPY ["HE.Investments.AHP.Allocation.Domain/HE.Investments.AHP.Allocation.Domain.csproj", "HE.Investments.AHP.Allocation.Domain/"]
COPY ["HE.Investments.AHP.Consortium.Contract/HE.Investments.AHP.Consortium.Contract.csproj", "HE.Investments.AHP.Consortium.Contract/"]
COPY ["HE.Investments.AHP.Consortium.Domain/HE.Investments.AHP.Consortium.Domain.csproj", "HE.Investments.AHP.Consortium.Domain/"]
COPY ["HE.Investments.AHP.ProjectDashboard.Contract/HE.Investments.AHP.ProjectDashboard.Contract.csproj", "HE.Investments.AHP.ProjectDashboard.Contract/"]
COPY ["HE.Investments.AHP.ProjectDashboard.Domain/HE.Investments.AHP.ProjectDashboard.Domain.csproj", "HE.Investments.AHP.ProjectDashboard.Domain/"]
COPY ["HE.Investments.Api/HE.Investments.Api.csproj", "HE.Investments.Api/"]
COPY ["HE.Investments.Common/HE.Investments.Common.csproj", "HE.Investments.Common/"]
COPY ["HE.Investments.Common.Contract/HE.Investments.Common.Contract.csproj", "HE.Investments.Common.Contract/"]
COPY ["HE.Investments.Common.CRM/HE.Investments.Common.CRM.csproj", "HE.Investments.Common.CRM/"]
COPY ["HE.Investments.Common.WWW/HE.Investments.Common.WWW.csproj", "HE.Investments.Common.WWW/"]
COPY ["HE.Investments.Consortium.Shared/HE.Investments.Consortium.Shared.csproj", "HE.Investments.Consortium.Shared/"]
COPY ["HE.Investments.DocumentService/HE.Investments.DocumentService.csproj", "HE.Investments.DocumentService/"]
COPY ["HE.Investments.FrontDoor.Shared/HE.Investments.FrontDoor.Shared.csproj", "HE.Investments.FrontDoor.Shared/"]
COPY ["HE.Investments.Organisation/HE.Investments.Organisation.csproj", "HE.Investments.Organisation/"]
COPY ["HE.Investments.Programme.Contract/HE.Investments.Programme.Contract.csproj", "HE.Investments.Programme.Contract/"]
COPY ["HE.Investments.Programme.Domain/HE.Investments.Programme.Domain.csproj", "HE.Investments.Programme.Domain/"]

RUN dotnet restore "./HE.Investment.AHP.WWW/./HE.Investment.AHP.WWW.csproj"

COPY . .
WORKDIR "/src/HE.Investment.AHP.WWW"
RUN dotnet build "./HE.Investment.AHP.WWW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./HE.Investment.AHP.WWW.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.Investment.AHP.WWW.dll"]
