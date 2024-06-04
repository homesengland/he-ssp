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
COPY ["HE.Investments.FrontDoor.WWW/HE.Investments.FrontDoor.WWW.csproj", "HE.Investments.FrontDoor.WWW/"]
COPY ["HE.Investments.FrontDoor.Contract/HE.Investments.FrontDoor.Contract.csproj", "HE.Investments.FrontDoor.Contract/"]
COPY ["HE.Investments.Common.Contract/HE.Investments.Common.Contract.csproj", "HE.Investments.Common.Contract/"]
COPY ["HE.Investments.FrontDoor.Domain/HE.Investments.FrontDoor.Domain.csproj", "HE.Investments.FrontDoor.Domain/"]
COPY ["HE.Investments.FrontDoor.Shared/HE.Investments.FrontDoor.Shared.csproj", "HE.Investments.FrontDoor.Shared/"]
COPY ["HE.Investments.Account.Shared/HE.Investments.Account.Shared.csproj", "HE.Investments.Account.Shared/"]
COPY ["HE.Investments.Account.Contract/HE.Investments.Account.Contract.csproj", "HE.Investments.Account.Contract/"]
COPY ["HE.Investments.Common.CRM/HE.Investments.Common.CRM.csproj", "HE.Investments.Common.CRM/"]
COPY ["HE.Investments.Common/HE.Investments.Common.csproj", "HE.Investments.Common/"]
COPY ["HE.Investments.Organisation/HE.Investments.Organisation.csproj", "HE.Investments.Organisation/"]
COPY ["HE.Investments.Common.WWW/HE.Investments.Common.WWW.csproj", "HE.Investments.Common.WWW/"]
COPY ["HE.Common.IntegrationModel.Crm/HE.Common.IntegrationModel.Crm.csproj", "HE.Common.IntegrationModel.Crm/"]
RUN dotnet restore "./HE.Investments.FrontDoor.WWW/./HE.Investments.FrontDoor.WWW.csproj"

COPY . .
WORKDIR "/src/HE.Investments.FrontDoor.WWW"
RUN dotnet build "./HE.Investments.FrontDoor.WWW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./HE.Investments.FrontDoor.WWW.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.Investments.FrontDoor.WWW.dll"]
