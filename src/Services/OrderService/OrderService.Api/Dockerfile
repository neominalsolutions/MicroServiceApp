#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/OrderService/OrderService.Api/OrderService.Api.csproj", "src/Services/OrderService/OrderService.Api/"]
COPY ["src/Services/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "src/Services/OrderService/OrderService.Infrastructure/"]
COPY ["src/Services/OrderService/OrderService.Application/OrderService.Application.csproj", "src/Services/OrderService/OrderService.Application/"]
COPY ["src/Services/OrderService/OrderService.Domain/OrderService.Domain.csproj", "src/Services/OrderService/OrderService.Domain/"]
RUN dotnet restore "src/Services/OrderService/OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/OrderService/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.Api.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]


