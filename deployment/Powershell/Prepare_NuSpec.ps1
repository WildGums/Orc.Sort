$fileName = $args[0]
$version = $args[1]

Write-Host "Using filename " $fileName
Write-Host "Using version " $version


$content = Get-Content $fileName
$content = $content -replace "\[VERSION\]", $version
Set-Content -Path $fileName -Value $content