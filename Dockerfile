FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/Payments.sln", "."]
COPY ["src/Payments.Api/Payments.Api.csproj", "Payments.Api/"]
COPY ["src/Payments.Application/Payments.Application.csproj", "Payments.Application/"]
COPY ["src/Payments.Domain/Payments.Domain.csproj", "Payments.Domain/"]
COPY ["src/Payments.Infra/Payments.Infra.csproj", "Payments.Infra/"]

# Restore dependencies
RUN dotnet restore "Payments.sln"

# Copy the rest of the source code
COPY ["src/", "."]

# Publish the application
WORKDIR "/src/Payments.Api"
RUN dotnet publish "Payments.Api.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5054
ENV ASPNETCORE_URLS=http://+:5054
ENTRYPOINT ["dotnet", "Payments.Api.dll"]
