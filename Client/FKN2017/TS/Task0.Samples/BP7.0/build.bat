@echo off

%~d0
cd "%~dp0"


set BP_HOME=P:\BP

set BPC="%BP_HOME%\BIN\BPC.EXE"


%BPC% Task0.pas
