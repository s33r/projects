# $OutputEncoding = [ System.Text.Encoding]::UTF8-BOM 

$valid = [bool]$true


Write-Host "    ╭─────────╮"
Write-Host "█───┤ Node.js ├──────█"
Write-Host "    ╰─────────╯"

node -v

$valid = $valid -and $?



Write-Host "    ╭─────────╮"
Write-Host "█───┤ .Net    ├──────█"
Write-Host "    ╰─────────╯"

dotnet --info

$valid = $valid -and $?


$result = "";
if($valid) {
    $result = '✅ Valid  '
} else {
    $result = '❌ Invalid'
}


$valid = $valid.ToString().PadLeft(8, [char]20)

Write-Host '┌─────────────┬────────────╮'
Write-Host "│ Environment │ ${result} │"
Write-Host '└─────────────┴────────────╯'
