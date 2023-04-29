# Script for backing minecraft server

# Global parameters
$daysToKeepBackup = 7

# Set Paths
$serverLoc = "PATH TO MINECRAFT SERVER FOLDER"
$backupLoc = "PATH TO STORE BACKUP"
$tempLoc   = New-Item -ItemType Directory -Path ("$env:Temp\" + "$([System.GUID]::NewGuid())")

# Backup the server
Copy-Item $serverLoc $tempLoc -Recurse -ErrorAction SilentlyContinue # if server is running it throws an error due to the session.lock file
Compress-Archive -Path "$tempLoc\$($serverLoc.Split("\")[-1])" -DestinationPath ("$backupLoc\{0:yyyy-MM-dd-HH-mm}.zip" -f (Get-Date)) -Force -Confirm:$false

# Clean up temp
Remove-Item $tempLoc -Recurse -Force -Confirm:$false

# Clean up old backups
Get-ChildItem –Path $backupLoc –Recurse | Where-Object CreationTime –lt (Get-Date).AddDays(-$daysToKeepBackup) | Remove-Item -Force -Confirm:$false