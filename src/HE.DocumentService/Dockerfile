FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base

RUN groupadd -g 999 appuser && \
 useradd -r -u 999 -g appuser appuser
USER appuser

WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

WORKDIR /src
COPY ["HE.DocumentService.Api/HE.DocumentService.Api.csproj", "HE.DocumentService.Api/"]
COPY ["HE.DocumentService.SharePoint/HE.DocumentService.SharePoint.csproj", "HE.DocumentService.SharePoint/"]
RUN dotnet restore "./HE.DocumentService.Api/HE.DocumentService.Api.csproj"
COPY . .
WORKDIR "/src/HE.DocumentService.Api"
RUN dotnet build "./HE.DocumentService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./HE.DocumentService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.DocumentService.Api.dll"]

