#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HE.InvestmentLoans.WWW/HE.InvestmentLoans.WWW.csproj", "HE.InvestmentLoans.WWW/"]
COPY ["He.AspNetCore.Mvc.Gds.Components/He.AspNetCore.Mvc.Gds.Components.csproj", "He.AspNetCore.Mvc.Gds.Components/"]
COPY ["HE.InvestmentLoans.BusinessLogic/HE.InvestmentLoans.BusinessLogic.csproj", "HE.InvestmentLoans.BusinessLogic/"]
COPY ["HE.InvestmentLoans.CRM/HE.InvestmentLoans.CRM.csproj", "HE.InvestmentLoans.CRM/"]
COPY ["HE.InvestmentLoans.Common/HE.InvestmentLoans.Common.csproj", "HE.InvestmentLoans.Common/"]
RUN dotnet restore "HE.InvestmentLoans.WWW/HE.InvestmentLoans.WWW.csproj"
COPY . .
WORKDIR "/src/HE.InvestmentLoans.WWW"
RUN dotnet build "HE.InvestmentLoans.WWW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HE.InvestmentLoans.WWW.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HE.InvestmentLoans.WWW.dll"]
