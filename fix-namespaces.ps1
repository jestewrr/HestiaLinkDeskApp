# Fix namespaces in all Razor files
$files = Get-ChildItem -Path "c:\Users\Jester\source\repos\HestiaIT13Final\Components" -Filter *.razor -Recurse

foreach ($file in $files) {
    Write-Host "Processing: $($file.FullName)"
    
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    
    # Remove redundant using statements (these are now in _Imports.razor)
    $content = $content -replace '@using HestiaLink\.Components\.Layout\r?\n', ''
    $content = $content -replace '@using HestiaLink\.Data\r?\n', ''
    $content = $content -replace '@using HestiaLink\.Models\r?\n', ''  
    $content = $content -replace '@using HestiaLink\.Services\r?\n', ''
    $content = $content -replace '@using HestiaLink\.Components\.Pages\r?\n', ''
    
    Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
}

Write-Host "Done! All Razor files have been updated."
