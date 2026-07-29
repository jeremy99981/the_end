#!/usr/bin/env bash
set -euo pipefail

project_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
dotnet_command="${DOTNET:-dotnet}"
output_dir="$project_root/artifacts/TheEnd-Windows-x64"

if ! command -v "$dotnet_command" >/dev/null 2>&1; then
  echo "dotnet SDK introuvable. Installez le SDK .NET 8 ou définissez DOTNET vers son exécutable." >&2
  exit 1
fi

rm -rf "$output_dir"
mkdir -p "$output_dir"

"$dotnet_command" publish "$project_root/src/TheEnd.App/TheEnd.App.csproj" \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  -p:EnableWindowsTargeting=true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:DebugType=None \
  -o "$output_dir"

mv "$output_dir/TheEnd.App.exe" "$output_dir/TheEnd.exe"
echo "Exécutable créé : $output_dir/TheEnd.exe"
