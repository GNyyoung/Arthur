set ExcelPath=%cd%
set ExePath=%ExcelPath%\..\..\..\Excel2Json

%ExePath%\Excel2Json.exe 
%ExePath%\Excel2Json -f %ExcelPath%\Monster.xlsx -o %ExcelPath%

:: *-f�� '�������ϸ�' = '��ġ���ϸ�'�̾�� ��*

pause