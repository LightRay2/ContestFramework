@echo off

%~d0
cd "%~dp0"

if not exist ".\1. ������" (
  echo 123
  exit
  O:
  cd O:\
)


cd ".\1. ������"
start /b "1. ������" /d .\lib .\lib\Client.exe

exit
