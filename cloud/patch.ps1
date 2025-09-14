$root = 'C:\Spatial'

$patch = Join-Path $root 'patch'
$build = Join-Path $root 'build'
$backup = Join-Path $root 'backup'

net stop spatial

if (Test-Path $backup) {
    Remove-Item -Recurse -Force $backup
}

if (Test-Path $build) {
    Copy-Item -Path $build -Destination $backup -Recurse -Force
    Remove-Item -Recurse -Force $build
}

if (-Not (Test-Path $build)) {
    New-Item -ItemType Directory -Path $build
}

Get-ChildItem -Path $patch -Force | ForEach-Object {
    Move-Item -Path $_.FullName -Destination $build -Force
}

Remove-Item -Recurse -Force $patch

net start spatial
