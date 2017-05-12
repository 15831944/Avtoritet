@ECHO OFF
start /MIN \\avtoritet24.com\epc\rapps\fordeu\MCFORDEU.rdp
exit

start regedit /s C:\Scripts\rus.reg
taskkill /F /T /IM ntvdm.exe
set delay=10
rem set current_date=%date:~-10%
rem date 30.02.2014
date 15.10.2015
start c:\Microcat\MICROCAT.EXE
rem start c:\Microcat\ford_microcat_eu.vbs
cd C:\Debug.Launcher\Root\Ford\

copy /y DSET.DAT C:\Microcat\mcindex\FEU
start ford_microcat_eu.vbs
@ping -n %delay% 127.0.0.1 > nul
date %current_date%
exit

