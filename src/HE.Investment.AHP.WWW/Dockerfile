ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN curl -Lk "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"


WORKDIR /src
COPY ["nuget.config", "."]
COPY ["HE.Investment.AHP.WWW/HE.Investment.AHP.WWW.csproj", "HE.Investment.AHP.WWW/"]
COPY ["HE.Investment.AHP.Contract/HE.Investment.AHP.Contract.csproj", "HE.Investment.AHP.Contract/"]
COPY ["HE.Investments.Common/HE.Investments.Common.csproj", "HE.Investments.Common/"]
COPY ["HE.Investment.AHP.Domain/HE.Investment.AHP.Domain.csproj", "HE.Investment.AHP.Domain/"]
COPY ["HE.Investments.Account.Shared/HE.Investments.Account.Shared.csproj", "HE.Investments.Account.Shared/"]
COPY ["HE.Investments.Account.Contract/HE.Investments.Account.Contract.csproj", "HE.Investments.Account.Contract/"]
COPY ["HE.Investments.Common.CRM/HE.Investments.Common.CRM.csproj", "HE.Investments.Common.CRM/"]
COPY ["HE.Investments.Organisation/HE.Investments.Organisation.csproj", "HE.Investments.Organisation/"]
COPY ["HE.Investments.Common.WWW/HE.Investments.Common.WWW.csproj", "HE.Investments.Common.WWW/"]
COPY ["HE.Investments.Loans.Common/HE.Investments.Loans.Common.csproj", "HE.Investments.Loans.Common/"]
COPY ["HE.Investments.DocumentService/HE.Investments.DocumentService.csproj", "HE.Investments.DocumentService/"]
RUN dotnet restore "./HE.Investment.AHP.WWW/./HE.Investment.AHP.WWW.csproj"
COPY . .
WORKDIR "/src/HE.Investment.AHP.WWW"
RUN dotnet build "./HE.Investment.AHP.WWW.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./HE.Investment.AHP.WWW.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.Investment.AHP.WWW.dll"]
