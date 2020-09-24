set CURPATH=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %CURPATH%\CometStage.xlsx -o ..\..\Assets\Datas\Stage

pause