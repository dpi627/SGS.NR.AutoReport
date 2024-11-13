@echo off
:: Code Page 改為英文
chcp 850
:: 取得當前目錄名稱 (通常等於方案、專案名稱)
for %%I in (.) do set "folderName=%%~nxI"
:: 設定專案後綴名稱 (預設留白)
set "projectSuffix=.Wpf"
:: 部署專案名稱，通常等於目錄名稱，如不同自行修改
set "projectName=%folderName%%projectSuffix%"
:: VS編譯後檔案所在目錄，如為 Net4 可抓 bin\Release
set "sourceDir=bin\publish"
:: 部署目標目錄
set "targetDir=\\twfs007\SGSSHARE\OAD\Brian\_Publish"
:: 完整來源路徑
set "sourcePath=%folderName%%projectSuffix%\%sourceDir%"
:: 完整部署路徑
set "targetPath=%targetDir%\%projectName%"
:: log 目錄名稱
set "logDir=Log"
:: log 檔案名稱，日期會受到前面 Code Page 影響，要改一起改
set "logFile=%logDir%\%DATE:~3,4%%DATE:~8,2%%DATE:~11,2%.log"

:: 建立 log 目錄 (如不存在 robocopy 會出錯)
if not exist "%logDir%" (
    mkdir "%logDir%"
    echo Log directory %logDir% created
)

robocopy %sourcePath% %targetPath% /mir /tee /r:1 /w:0 /log+:%logFile%.log

echo Press any key to exit...
pause > nul