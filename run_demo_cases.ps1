$ErrorActionPreference = "Stop"

Set-Location $PSScriptRoot

$cases = @(
    @{
        Name = "Case 1"
        Query = 'status = "open" and owner = "sam"'
        Expected = "true"
    },
    @{
        Name = "Case 2"
        Query = 'priority >= 3 or score > 90'
        Expected = "true"
    },
    @{
        Name = "Case 3"
        Query = 'not archived and (priority >= 3 or owner = "max")'
        Expected = "false"
    }
)

foreach ($case in $cases) {
    Write-Host ""
    Write-Host "=== $($case.Name) ===" -ForegroundColor Cyan
    Write-Host "Query:    $($case.Query)"
    Write-Host "Expected: $($case.Expected)"

    $tmpFile = Join-Path $PSScriptRoot "_demo_input.txt"
    Set-Content -Path $tmpFile -Value $case.Query -NoNewline

    try {
        dotnet run -- $tmpFile
    }
    finally {
        if (Test-Path $tmpFile) {
            Remove-Item $tmpFile -Force
        }
    }
}
