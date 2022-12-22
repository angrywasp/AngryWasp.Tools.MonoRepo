#!/bin/bash -e

if [[ -z "${PKG_VER}" ]]; then
    echo "PKG_VER env var not present. Aborting"
    exit
fi

dotnet build -c Release Src/AngryWasp.Tools.MonoRepo.sln -p:PackageVersion=${PKG_VER}
dotnet nuget push Bin/Binaries/AngryWasp.${1}.${PKG_VER}.nupkg --source https://api.nuget.org/v3/index.json
