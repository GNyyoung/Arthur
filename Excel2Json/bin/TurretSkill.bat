set ExcelPath=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %ExcelPath%\TurretSkill.xlsx -o ..\..\Assets\Datas

pause