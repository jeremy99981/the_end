#!/usr/bin/env bash
set -euo pipefail

project_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
nsis_command="${MAKENSIS:-makensis}"
app_version="${APP_VERSION:-1.4.0}"

if ! command -v "$nsis_command" >/dev/null 2>&1; then
  echo "makensis introuvable. Installez NSIS ou définissez MAKENSIS vers son exécutable." >&2
  exit 1
fi

if [[ ! -f "$project_root/artifacts/RecapBrun-Windows-x64/RecapBrun.exe" ]]; then
  echo "RecapBrun.exe absent. Lancez d’abord scripts/build-windows.sh." >&2
  exit 1
fi

mkdir -p "$project_root/artifacts"
"$nsis_command" -V4 "-DPROJECT_ROOT=$project_root" "-DAPP_VERSION=$app_version" "$project_root/installer/RecapBrun.nsi"
echo "Installateur créé : $project_root/artifacts/RecapBrun-Setup-$app_version.exe"
