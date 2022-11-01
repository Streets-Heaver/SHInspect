Param(
    [string]$kvu,
    [string]$kvi,
    [string]$kvs,
    [string]$kvc,
    [string]$tenantId

)

dotnet tool update --global AzureSignTool --ignore-failed-sources

New-Item "$Env:Build_ArtifactStagingDirectory\signsquirrel.txt"

$filter = "*.dll", "*.exe", "*.msi"
$files = Get-ChildItem -path "$env:BUILD_ARTIFACTSTAGINGDIRECTORY\Squirrel\*" -Include $filter -Recurse
foreach ($t in $files) {
    Add-Content -Path "$Env:Build_ArtifactStagingDirectory\signsquirrel.txt" -Value $t.FullName
}

Write-Host "Starting Sign"

AzureSignTool sign -kvu $kvu -kvi $kvi -kvs $kvs -kvc $kvc -v -ifl "$Env:Build_ArtifactStagingDirectory\signsquirrel.txt" --azure-key-vault-tenant-id $tenantId