@echo off

%~d0
cd "%~dp0"

if not exist ".\TS" (
  O:
  cd O:\
)


cd ".\5. ���� � 诨���"
start /b "5. ���� � 诨���" "."
::explorer /n, "."

exit
