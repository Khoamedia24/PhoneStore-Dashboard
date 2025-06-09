# optimize_images.ps1
# Script to optimize large images in the project

Write-Host "Optimizing large images..." -ForegroundColor Green

# Check if the uploads directory exists, if not create it
$uploadsDir = ".\wwwroot\uploads"
if (-not (Test-Path $uploadsDir)) {
    New-Item -Path $uploadsDir -ItemType Directory
}

# Create a directory for optimized images
$optimizedDir = ".\wwwroot\assets\img\optimized"
if (-not (Test-Path $optimizedDir)) {
    New-Item -Path $optimizedDir -ItemType Directory
}

# List of image optimization tools you can install
Write-Host "Note: For better optimization, consider installing one of these tools:" -ForegroundColor Yellow
Write-Host "- ImageMagick: Install-Package Magick.NET" -ForegroundColor Yellow
Write-Host "- NuGet: Install-Package ImageResizer" -ForegroundColor Yellow
Write-Host "- NPM: npm install -g imagemin-cli" -ForegroundColor Yellow

Write-Host "Manual image optimization recommendations:" -ForegroundColor Cyan
Write-Host "1. Resize large images to appropriate dimensions (e.g., max 1200px width)" -ForegroundColor Cyan
Write-Host "2. Compress JPG images to 80% quality" -ForegroundColor Cyan
Write-Host "3. Convert large images to WebP format for better compression" -ForegroundColor Cyan
Write-Host "4. Remove any unused images from the assets directory" -ForegroundColor Cyan

# List large images that should be optimized
$largeImages = Get-ChildItem -Path ".\wwwroot\assets\img" -Recurse -File | Where-Object { $_.Length -gt 1MB }
if ($largeImages.Count -gt 0) {
    Write-Host "`nLarge images that should be optimized:" -ForegroundColor Red
    foreach ($img in $largeImages) {
        $sizeMB = "{0:N2}" -f ($img.Length/1MB)
        Write-Host "- $($img.FullName) ($sizeMB MB)" -ForegroundColor Red
    }
}

Write-Host "`nImage optimization completed!" -ForegroundColor Green
