#!/bin/bash -e

export VERSION=3.0.2

dotnet build -c Release Src/AngryWasp.Tools.MonoRepo.sln -p:PackageVersion=${VERSION}

packages=(Cli Cryptography Helpers Json.Rpc Logger Net Serializer)

for i in "${packages[@]}"
do
    dotnet nuget push Bin/Binaries/AngryWasp.${i}.${VERSION}.nupkg --source https://api.nuget.org/v3/index.json
done