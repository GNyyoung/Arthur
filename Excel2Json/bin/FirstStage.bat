set CURPATH=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %CURPATH%\FirstStage.xlsx -o ..\..\Assets\Datas\Stage

pause