Param(
    [string]$kvu,
    [string]$kvi,
    [string]$kvs,
    [string]$kvc,
    [string]$tenant
)

dotnet tool update --global AzureSignTool

New-Item "$Env:Build_ArtifactStagingDirectory\sign.txt"

$filter = "*.dll", "*.exe", "*.msi"
$files = Get-ChildItem -path "$Env:Build_SourcesDirectory\SHInspect\bin\Release\*" -Include $filter -Recurse
foreach ($t in $files) {
    Add-Content -Path "$Env:Build_ArtifactStagingDirectory\sign.txt" -Value $t.FullName
}

Write-Host "Starting Sign"

AzureSignTool sign -kvu $kvu -kvi $kvi -kvs $kvs -kvc $kvc -v -ifl "$Env:Build_ArtifactStagingDirectory\sign.txt" --azure-key-vault-tenant-id $tenant
