# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/FaithFramework.Sample.Doom/*" -Force -Recurse
dotnet publish "./FaithFramework.Sample.Doom.csproj" -c Release -o "$env:RELOADEDIIMODS/FaithFramework.Sample.Doom" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location