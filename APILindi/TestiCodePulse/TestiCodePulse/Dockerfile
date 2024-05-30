# Use the official ASP.NET image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestiCodePulse", "/src/TestiCodePulse"]
WORKDIR "/src/TestiCodePulse"
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Copy the Images directory into the Docker image
COPY ["APILindi/TestiCodePulse/TestiCodePulse/Images", "/app/Images"]

# Build the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage / production stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TestiCodePulse.dll"]
