with import <nixpkgs> {};
  buildDotnetModule rec {
    pname = "ConsoleLogger";
    version = "0.1";
    src = ./.;
    projectFile = "./src/Server/Server.csproj";
    # nugetDeps = ./deps.json;
    dotnet-sdk = dotnetCorePackages.sdk_9_0;
    dotnet-runtime = dotnetCorePackages.runtime_9_0;
    selfContainedBuild = true;
    # useDotnetFromEnv = true;
  }
