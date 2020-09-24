set ExcelPath=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %ExcelPath%\Turret.xlsx -o ..\..\Assets\Datas

pause