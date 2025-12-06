# Dockerfile for SysDocs
# Multi-stage build for minimal, reproducible container images

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY src/Directory.Build.props .
COPY src/SysDocs.sln .
COPY src/SysDocs.Core/SysDocs.Core.csproj ./SysDocs.Core/
COPY src/SysDocs.Cli/SysDocs.Cli.csproj ./SysDocs.Cli/
COPY src/SysDocs.Templates/SysDocs.Templates.csproj ./SysDocs.Templates/
COPY src/SysDocs.Tests/SysDocs.Tests.csproj ./SysDocs.Tests/

# Restore dependencies
RUN dotnet restore SysDocs.sln

# Copy source code
COPY src/ .

# Build and test
RUN dotnet build SysDocs.sln -c Release --no-restore
RUN dotnet test SysDocs.Tests/SysDocs.Tests.csproj -c Release --no-build --verbosity normal

# Publish
RUN dotnet publish SysDocs.Cli/SysDocs.Cli.csproj \
    -c Release \
    --no-restore \
    -o /app/publish \
    /p:Deterministic=true \
    /p:ContinuousIntegrationBuild=true

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

# Create non-root user for security
RUN useradd -m -u 1000 sysdocs

# Install necessary dependencies for deterministic rendering
RUN apt-get update && apt-get install -y \
    fontconfig \
    libfreetype6 \
    libfontconfig1 \
    && rm -rf /var/lib/apt/lists/*

# Copy embedded fonts for deterministic output
COPY src/SysDocs.Templates/Fonts/ /usr/share/fonts/sysdocs/
RUN fc-cache -fv

WORKDIR /app
COPY --from=build /app/publish .

# Set ownership
RUN chown -R sysdocs:sysdocs /app

# Switch to non-root user
USER sysdocs

# Set environment variables for deterministic behavior
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    TZ=UTC

# Volume for input/output
VOLUME ["/workspace"]
WORKDIR /workspace

ENTRYPOINT ["dotnet", "/app/sysdocs.dll"]
CMD ["--help"]
