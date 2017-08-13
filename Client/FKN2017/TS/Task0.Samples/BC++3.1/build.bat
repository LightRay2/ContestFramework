@echo off

%~d0
cd "%~dp0"


set BC_HOME=P:\BC

set BCC="%BC_HOME%\BIN\BCC.EXE" -I%BC_HOME%\INCLUDE
set TLINK="%BC_HOME%\BIN\TLINK.EXE" /L%BC_HOME%\LIB


set MEM_MODEL=l

%BCC% -c -P -D__MEM_H -m%MEM_MODEL% Task0.cpp
%TLINK% c0%MEM_MODEL%.obj Task0.obj , Task0.exe , , c%MEM_MODEL%.lib
del *.obj *.map
