@echo off

%~d0
cd "%~dp0"

if not exist ".\TS" (
  O:
  cd O:\
)


cd ".\TS"
start /b "9. ��䬥��᪮� ��ࠦ����" .\TS.exe

exit
