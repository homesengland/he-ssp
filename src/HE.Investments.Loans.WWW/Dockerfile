FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base

RUN groupadd -g 999 appuser && \
 useradd -r -u 999 -g appuser appuser
USER appuser

WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:8.0.204 AS build
WORKDIR /src

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["He.AspNetCore.Mvc.Gds.Components/He.AspNetCore.Mvc.Gds.Components.csproj", "He.AspNetCore.Mvc.Gds.Components/"]
COPY ["HE.Common.IntegrationModel.Crm/HE.Common.IntegrationModel.Crm.csproj", "HE.Common.IntegrationModel.Crm/"]
COPY ["HE.Investments.Account.Api.Contract/HE.Investments.Account.Api.Contract.csproj", "HE.Investments.Account.Api.Contract/"]
COPY ["HE.Investments.Account.Shared/HE.Investments.Account.Shared.csproj", "HE.Investments.Account.Shared/"]
COPY ["HE.Investments.Api/HE.Investments.Api.csproj", "HE.Investments.Api/"]
COPY ["HE.Investments.Common/HE.Investments.Common.csproj", "HE.Investments.Common/"]
COPY ["HE.Investments.Common.Contract/HE.Investments.Common.Contract.csproj", "HE.Investments.Common.Contract/"]
COPY ["HE.Investments.Common.CRM/HE.Investments.Common.CRM.csproj", "HE.Investments.Common.CRM/"]
COPY ["HE.Investments.Common.WWW/HE.Investments.Common.WWW.csproj", "HE.Investments.Common.WWW/"]
COPY ["HE.Investments.DocumentService/HE.Investments.DocumentService.csproj", "HE.Investments.DocumentService/"]
COPY ["HE.Investments.FrontDoor.Shared/HE.Investments.FrontDoor.Shared.csproj", "HE.Investments.FrontDoor.Shared/"]
COPY ["HE.Investments.Loans.BusinessLogic/HE.Investments.Loans.BusinessLogic.csproj", "HE.Investments.Loans.BusinessLogic/"]
COPY ["HE.Investments.Loans.Common/HE.Investments.Loans.Common.csproj", "HE.Investments.Loans.Common/"]
COPY ["HE.Investments.Loans.Contract/HE.Investments.Loans.Contract.csproj", "HE.Investments.Loans.Contract/"]
COPY ["HE.Investments.Loans.WWW/HE.Investments.Loans.WWW.csproj", "HE.Investments.Loans.WWW/"]
COPY ["HE.Investments.Organisation/HE.Investments.Organisation.csproj", "HE.Investments.Organisation/"]

RUN dotnet restore "./HE.Investments.Loans.WWW/./HE.Investments.Loans.WWW.csproj"
COPY . .
WORKDIR "/src/HE.Investments.Loans.WWW"
RUN dotnet build "./HE.Investments.Loans.WWW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./HE.Investments.Loans.WWW.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.Investments.Loans.WWW.dll"]
