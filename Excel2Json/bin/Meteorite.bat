set ExcelPath=%cd%

..\Excel2Json.exe 
..\Excel2Json -f %ExcelPath%\Meteorite.xlsx -o ..\..\Assets\Datas

pause