@echo off

%~d0
cd "%~dp0"

if not exist ".\1. Уголки" (
  echo 123
  exit
  O:
  cd O:\
)


cd ".\1. Уголки"
start /b "1. Уголки" /d .\lib .\lib\Client.exe

exit
