set ExcelPath=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %ExcelPath%\TurretLevelCost.xlsx -o ..\..\Assets\Datas

pause