param(
    [switch]$build = $false
)

$base="docker-compose -f docker-compose.yml -f docker-compose.override.yml up"

if ($build) {
    Invoke-Expression "$base --build"
} else {
    Invoke-Expression "$base"
}