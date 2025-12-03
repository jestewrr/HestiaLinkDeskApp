$replacements = @{
    '?? Loading' = ' Loading'
    '?? No ' = ' No '
    '?? View Archived' = ' View Archived'
    '?? View Inactive' = ' View Inactive'
    '?? Archived' = ' Archived'
    '?? Edit' = ' Edit'
    '?? Archive' = ' Archive'
    '?? Restore' = ' Restore'
    '?? Activate' = ' Activate'
    '??? Delete' = ' Delete'
    '??? Confirm' = ' Confirm'
    '<span class="search-icon">??</span>' = '<span class="search-icon"></span>'
    '<span class="error-icon">??</span>' = '<span class="error-icon"></span>'
    '<div class="no-users-icon">??</div>' = '<div class="no-users-icon"></div>'
    'title="Edit" @onclick="() => OpenEditModal(dept)">??' = 'title="Edit" @onclick="() => OpenEditModal(dept)">'
    'title="Archive" @onclick="() => ArchiveDepartment(dept)">??' = 'title="Archive" @onclick="() => ArchiveDepartment(dept)">'
}

$files = Get-ChildItem -Path "c:\Users\Jester\source\repos\HestiaIT13Final\Components\Pages" -Filter *.razor -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $modified = $false
    
    foreach ($key in $replacements.Keys) {
        if ($content -match [regex]::Escape($key)) {
            $content = $content -replace [regex]::Escape($key), $replacements[$key]
            $modified = $true
        }
    }
    
    if ($modified) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
        Write-Host "Updated: $($file.Name)" -ForegroundColor Green
    }
}

Write-Host "Icon replacement complete!" -ForegroundColor Cyan
