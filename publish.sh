#!/bin/bash
dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

function getNewVersion()
{
    pkg=$1
    lvl=$2

    version=$(curl -s https://api.nuget.org/v3-flatcontainer/AngryWasp.${pkg}/index.json | jq .[][-1] | tr -d '"')
    semver=(${version//./ })

    if [ "${lvl}" == "major" ]; then
        ((semver[0]++))
        semver[1]=0
        semver[2]=0
    elif [ "${lvl}" == "minor" ]; then
        ((semver[1]++))
        semver[2]=0
    else
        ((semver[2]++))
    fi

    echo "${semver[0]}.${semver[1]}.${semver[2]}"
}

version=$(getNewVersion $1 patch)

dotnet build -c Release Src/AngryWasp.Tools.MonoRepo.sln -p:PackageVersion=${version}
dotnet nuget push Bin/Binaries/AngryWasp.${1}.${version}.nupkg --source https://api.nuget.org/v3/index.json
