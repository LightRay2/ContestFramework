@echo off

%~d0
cd "%~dp0"

if not exist ".\TS" (
  O:
  cd O:\
)


cd ".\5. Ксюша и шпионы"
start /b "5. Ксюша и шпионы" "."
::explorer /n, "."

exit
