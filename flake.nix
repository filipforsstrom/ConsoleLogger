{
  description = "dotnet environment";
  inputs.flake-utils.url = "github:numtide/flake-utils";
  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

  outputs = {
    self,
    flake-utils,
    nixpkgs,
  }:
    flake-utils.lib.eachDefaultSystem (
      system: let
        pkgs = nixpkgs.legacyPackages.${system};
      in {
        devShell = pkgs.mkShell {
          packages = with pkgs; [
            dotnetCorePackages.sdk_9_0
            nuget-to-json
          ];
          buildInputs = with pkgs; [
          ];
          shellHook = ''
            export DOTNET_ROOT=${pkgs.dotnetCorePackages.sdk_9_0}
            export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 #workaround for c# dev kit
          '';
        };
      }
    );
}
