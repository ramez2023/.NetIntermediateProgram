#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["scr/WEBAPI.Api/WEBAPI.Api.csproj", "scr/WEBAPI.Api/"]
COPY ["scr/WEBAPI.Infrastructure/WEBAPI.Infrastructure.csproj", "scr/WEBAPI.Infrastructure/"]
COPY ["scr/WEBAPI.Common/WEBAPI.Common.csproj", "scr/WEBAPI.Common/"]
COPY ["scr/WEBAPI.Domain/WEBAPI.Domain.csproj", "scr/WEBAPI.Domain/"]
COPY ["scr/WEBAPI.Service/WEBAPI.Service.csproj", "scr/WEBAPI.Service/"]
RUN dotnet restore "./scr/WEBAPI.Api/./WEBAPI.Api.csproj"
COPY . .
WORKDIR "/src/scr/WEBAPI.Api"
RUN dotnet build "./WEBAPI.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WEBAPI.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WEBAPI.Api.dll"]