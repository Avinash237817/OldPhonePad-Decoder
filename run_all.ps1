param(
    [int]$NumCases = 1000,
    [switch]$UseUploadedDecoder
)

$RepoRoot = Split-Path -Path $MyInvocation.MyCommand.Path -Parent
Write-Host "Repo root: $RepoRoot"

# Optional replacement with uploaded decoder
if ($UseUploadedDecoder) {
    $uploaded = "/mnt/data/OldPhonePad.cs"
    $target = Join-Path $RepoRoot "src\OldPhonePad\OldPhonePadDecoder.cs"
    if (Test-Path $uploaded) {
        Copy-Item -Path $uploaded -Destination $target -Force
        Write-Host "Replaced decoder with uploaded file."
    } else {
        Write-Warning "Uploaded decoder not found."
    }
}

Write-Host ""
Write-Host "=== Building src ==="
dotnet build "$RepoRoot\src\OldPhonePad\OldPhonePad.csproj" -c Release

Write-Host ""
Write-Host "=== Building tests ==="
dotnet build "$RepoRoot\tests\TestHarness.csproj" -c Release

Write-Host ""
Write-Host "=== Generating $NumCases testcases ==="
dotnet run --project "$RepoRoot\tests\TestHarness.csproj" -- --gen $NumCases

Write-Host ""
Write-Host "=== Searching for generated testcases.csv ==="
$testcases = Get-ChildItem -Path $RepoRoot -Recurse -Filter testcases.csv | Select-Object -First 1

if (-not $testcases) {
    Write-Error "testcases.csv not found!"
    exit 2
}

$testcasesPath = $testcases.FullName
Write-Host "Found: $testcasesPath"

$resultsPath = Join-Path $RepoRoot "tests\results.csv"

Write-Host ""
Write-Host "=== Running decoder on generated CSV ==="
dotnet run --project "$RepoRoot\src\OldPhonePad\OldPhonePad.csproj" -- --file "$testcasesPath" --out "$resultsPath"

Write-Host ""
Write-Host "=== SUMMARY ==="

if (-not (Test-Path $resultsPath)) {
    Write-Error "Decoder did not produce results.csv!"
    exit 3
}

$rows = Import-Csv $resultsPath
$total = $rows.Count
$fail = ($rows | Where-Object { $_.match -ne "PASS" }).Count

Write-Host "Total cases: $total"
Write-Host "Failures: $fail"

if ($fail -eq 0) {
    Write-Host "✔ ALL TESTS PASSED"
} else {
    Write-Host "❌ Some tests failed — check tests/results.csv"
}
