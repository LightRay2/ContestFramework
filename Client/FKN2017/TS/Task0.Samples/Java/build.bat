@echo off

%~d0
cd "%~dp0"


set JAVA_HOME=C:\Program Files\Java\jdk1.6.0_11

set JAVA="%JAVA_HOME%\bin\java"
set JAVAC="%JAVA_HOME%\bin\javac"
set JAR="%JAVA_HOME%\bin\jar"


%JAVAC% *.java
%JAR% -cfe Task0.jar Task0 *.class
