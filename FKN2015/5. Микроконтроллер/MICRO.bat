@echo off

%~d0
cd "%~dp0"


if not exist ./input.txt (
  echo Не найден файл input.txt
  pause
  exit
)

set MICRO="O:\5. Микроконтроллер\task.exe"

%MICRO% ./input.txt
pause
exit