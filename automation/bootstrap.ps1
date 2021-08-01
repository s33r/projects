$Env:NODE_ENV = "development"

$command = 'Set-Alias -Name build -Value ./output/Aaron.Automation.Cli.exe -Force'
$profileLocation = "$Home\Documents\WindowsPowerShell\Microsoft.PowerShell_profile.ps1"

Add-Content -Path $profileLocation -Value ''
Add-Content -Path $profileLocation -Value $command