{
  description = "SysDocs - Deterministic Systems Engineering Documentation Pipeline";

  inputs = {
    # Pin to stable nixpkgs for maximum determinism
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config.allowUnfree = true;
        };

        # For cross-platform Docker builds, we need Linux packages
        pkgsLinux = import nixpkgs {
          system = "x86_64-linux";
          config.allowUnfree = true;
        };

        # Pin .NET 8 SDK version for determinism (until .NET 10 packages mature)
        dotnetSdk = pkgs.dotnet-sdk_8;
        
        # Deterministic build environment dependencies
        buildInputs = with pkgs; [
          dotnetSdk
          git
          fontconfig
          freetype
          libpng
          libjpeg
          libgit2
        ];

        # Version information
        version = "0.1.0-alpha";
        
        # Build the application package
        sysdocs = pkgs.stdenv.mkDerivation {
          pname = "sysdocs";
          inherit version;

          src = ./.;

          nativeBuildInputs = buildInputs;

          # Ensure deterministic builds
          SOURCE_DATE_EPOCH = "315532800"; # 1980-01-01 00:00:00 UTC

          buildPhase = ''
            export HOME=$(mktemp -d)
            export DOTNET_CLI_TELEMETRY_OPTOUT=1
            export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
            export DOTNET_NOLOGO=1
            
            # Restore dependencies with locked versions
            dotnet restore src/SysDocs.sln \
              --locked-mode \
              --verbosity normal
            
            # Build with deterministic flags
            dotnet build src/SysDocs.sln \
              -c Release \
              --no-restore \
              /p:Deterministic=true \
              /p:ContinuousIntegrationBuild=true \
              /p:SourceRevisionId=${self.rev or "dev"} \
              /p:SourceRoot=$src/
            
            # Run tests
            dotnet test tests/SysDocs.Tests/SysDocs.Tests.csproj \
              -c Release \
              --no-build \
              --verbosity normal
          '';

          installPhase = ''
            # Publish the CLI application
            dotnet publish src/SysDocs.Cli/SysDocs.Cli.csproj \
              -c Release \
              --no-restore \
              -o $out/lib/sysdocs \
              /p:Deterministic=true \
              /p:ContinuousIntegrationBuild=true \
              /p:SourceRevisionId=${self.rev or "dev"}
            
            mkdir -p $out/bin
            
            # Create wrapper script
            cat > $out/bin/sysdocs <<EOF
            #!/bin/sh
            exec ${dotnetSdk}/bin/dotnet $out/lib/sysdocs/sysdocs.dll "\$@"
            EOF
            
            chmod +x $out/bin/sysdocs
            
            # Copy templates and fonts
            mkdir -p $out/share/sysdocs
            cp -r src/SysDocs.Templates/Templates $out/share/sysdocs/
            cp -r src/SysDocs.Templates/Fonts $out/share/sysdocs/
          '';

          meta = with pkgs.lib; {
            description = "Deterministic Systems Engineering Documentation Pipeline";
            homepage = "https://github.com/CodeOOf/SysDocs";
            license = licenses.mit;
            maintainers = [ ];
            platforms = platforms.unix;
          };
        };

        # Build deterministic Docker image using Nix (works cross-platform)
        dockerImage = pkgsLinux.dockerTools.buildLayeredImage {
          name = "sysdocs";
          tag = version;
          
          # Use the Nix-built application
          contents = [
            sysdocs
            pkgsLinux.coreutils
            pkgsLinux.bash
            pkgsLinux.fontconfig
            pkgsLinux.freetype
            pkgsLinux.libpng
            pkgsLinux.libjpeg
            pkgsLinux.dotnet-runtime_8
          ];

          # Set up the container environment
          config = {
            Cmd = [ "${sysdocs}/bin/sysdocs" ];
            WorkingDir = "/workspace";
            Env = [
              "DOTNET_CLI_TELEMETRY_OPTOUT=1"
              "DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1"
              "DOTNET_NOLOGO=1"
              "FONTCONFIG_PATH=${pkgsLinux.fontconfig.out}/etc/fonts"
            ];
            User = "1000:1000";
            ExposedPorts = { };
            Volumes = {
              "/workspace" = { };
            };
          };

          # Ensure deterministic layer creation
          created = "1980-01-01T00:00:00Z";
          
          # Maximum compression for smaller images
          maxLayers = 100;
        };

      in
      {
        # Packages
        packages = {
          default = sysdocs;
          sysdocs = sysdocs;
          docker = dockerImage;
        };

        # Development shell
        devShells.default = pkgs.mkShell {
          buildInputs = buildInputs ++ (with pkgs; [
            # Additional development tools
            omnisharp-roslyn
            docker
            
            # Useful utilities
            jq
            yq
            tree
            
            # Markdown tools
            marksman
          ]);

          shellHook = ''
            echo "╔═══════════════════════════════════════════════════════════╗"
            echo "║  SysDocs Deterministic Development Environment           ║"
            echo "╚═══════════════════════════════════════════════════════════╝"
            echo ""
            echo "🔧 Build Commands:"
            echo "  nix build                    - Build SysDocs with Nix"
            echo "  nix build .#docker           - Build deterministic Docker image"
            echo "  dotnet build src/SysDocs.sln - Quick local build"
            echo "  dotnet test                  - Run test suite"
            echo ""
            echo "🐳 Docker Commands:"
            echo "  docker load < result         - Load Nix-built Docker image"
            echo "  docker run sysdocs:${version} - Run the container"
            echo ""
            echo "📦 Verification:"
            echo "  nix build .#docker && sha256sum result - Get image hash"
            echo ""
            echo "📖 Documentation:"
            echo "  See docs/DETERMINISTIC_BUILDS.md for details"
            echo ""
            echo ".NET SDK: ${dotnetSdk.version}"
            echo "System: ${system}"
            
            export DOTNET_CLI_TELEMETRY_OPTOUT=1
            export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
            export DOTNET_NOLOGO=1
          '';
        };

        # Apps
        apps.default = flake-utils.lib.mkApp {
          drv = sysdocs;
          exePath = "/bin/sysdocs";
        };

        # Formatter
        formatter = pkgs.nixpkgs-fmt;
      });
}
