FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;

# Adding missing certificate to allow access to githubusercontent.com below.
# We don't need to do this for build servers but when building locally the certificates required for
# checking the Certificate Revocation List are blocked by Homes England and the curl would fail.
#COPY "cisco_umbrella_root_ca.crt" "/usr/local/share/ca-certificates/cisco_umbrella_root_ca.crt"
#RUN chmod 644 /usr/local/share/ca-certificates/cisco_umbrella_root_ca.crt
#RUN update-ca-certificates

RUN curl -Lk "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

#WORKDIR /src
#COPY ["HE.InvestmentLoans.WWW/HE.InvestmentLoans.WWW.csproj", "HE.InvestmentLoans.WWW/"]
#COPY ["He.AspNetCore.Mvc.Gds.Components/He.AspNetCore.Mvc.Gds.Components.csproj", "He.AspNetCore.Mvc.Gds.Components/"]
#COPY ["HE.InvestmentLoans.BusinessLogic/HE.InvestmentLoans.BusinessLogic.csproj", "HE.InvestmentLoans.BusinessLogic/"]
#COPY ["HE.InvestmentLoans.CRM/HE.InvestmentLoans.CRM.csproj", "HE.InvestmentLoans.CRM/"]
#COPY ["HE.InvestmentLoans.Common/HE.InvestmentLoans.Common.csproj", "HE.InvestmentLoans.Common/"]

#COPY ["nuget.config", "./"]
#RUN dotnet restore "HE.InvestmentLoans.WWW/HE.InvestmentLoans.WWW.csproj" --configfile "./nuget.config"
COPY . .
RUN ls
WORKDIR "HE.InvestmentLoans.WWW"
RUN dotnet build "HE.InvestmentLoans.WWW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HE.InvestmentLoans.WWW.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.InvestmentLoans.WWW.dll"]