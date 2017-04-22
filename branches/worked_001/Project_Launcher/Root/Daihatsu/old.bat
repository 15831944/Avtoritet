@ECHO OFF
cd c:\MCDEU\
start regedit /s C:\Scripts\rus.reg 
taskkill /F /T /IM ntvdm.exe>nul
set delay=15
set current_date=%date:~-10%
date 26.10.2013
copy C:\MCDEU\launch\old_cars\Mg16.dll c:\MCDEU
start c:\MCDEU\mcdeu.exe
cd C:\Debug.Launcher\Root\Daihatsu
start dh.vbs
@ping -n %delay% 127.0.0.1 > nul
date %current_date%
exit