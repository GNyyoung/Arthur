set ExcelPath=%cd%
set ExePath=%ExcelPath%\..\..\..\Excel2Json

%ExePath%\Excel2Json.exe 
%ExePath%\Excel2Json -f %ExcelPath%\Test.xlsx -o %ExcelPath%

pause