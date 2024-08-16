$script = Join-Path $PSScriptRoot "build.cake"
dotnet cake $script @args
