@echo off

%~d0
cd "%~dp0"

if not exist ".\TS" (
  O:
  cd O:\
)


cd ".\TS"
start /b "8. ���祭�� �몮�" .\TS.exe

exit
