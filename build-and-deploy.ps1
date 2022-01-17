[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Release","Debug")]
    [String]$configuration
)

$versionFilePath = "./nuget/version.txt"

# Generate a new version number modifier
[int]$i = Get-Content $versionFilePath
$i++
Set-Content $versionFilePath $i
Write-Host "Using VersionModifier: $i"

# clean out old builds
Remove-Item package/NUnit3TestAdapter* -Force -Recurse

& ./build.ps1 -t Package -Configuration $configuration -VersionModifier $i;


$packageFile = (Get-ChildItem -Path package/ -Recurse -Filter "NUnit3TestAdapter*.nupkg")[0].Name

& nuget add package/$packageFile -source \\sta.cwserverfarm.local\WorkstationDeploy\cwEnvConfig\Software\DeveloperWorkstation\nuget


