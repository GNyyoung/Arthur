set ExcelPath=%cd%
set ExePath=%ExcelPath%\..\..\..\Excel2Json

%ExePath%\Excel2Json.exe 
%ExePath%\Excel2Json -f %ExcelPath%\Sword.xlsx -o %ExcelPath%

:: *-f의 '엑셀파일명' = '배치파일명'이어야 함*

pause