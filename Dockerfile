FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Core.DomainServices/Core.DomainServices.csproj", "Core.DomainServices/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["RestApi/RestApi.csproj", "RestApi/"]
RUN dotnet restore "RestApi/RestApi.csproj"
COPY . .
WORKDIR "/src/RestApi"
RUN dotnet build "RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet RestApi.dll