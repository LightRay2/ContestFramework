@echo off

%~d0
cd "%~dp0"


if not exist ./input.txt (
  echo �� ������ 䠩� input.txt
  pause
  exit
)

set MICRO="O:\5. ���ப���஫���\task.exe"

%MICRO% ./input.txt
pause
exit