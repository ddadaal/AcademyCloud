#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Identity/Identity.Service/Identity.Service.csproj", "Identity/Identity.Service/"]
RUN dotnet restore "Identity/Identity.Service/Identity.Service.csproj"
COPY . .
WORKDIR "/src/Identity/Identity.Service"
RUN dotnet build "Identity.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./wait-for-it.sh .
ENTRYPOINT ["./wait-for-it.sh", "db:3306", "-t", "60000", "--", "dotnet", "Identity.Service.dll"]