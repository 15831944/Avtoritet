@ECHO OFF
start regedit /s C:\Scripts\rus.reg
taskkill /F /T /IM ntvdm.exe
set delay=20
set current_date=%date:~-10%
date 20.09.2008
start c:\Mcgmc\MCGMC.EXE
start c:\Debug.Launcher\Root\Gm\gm_all.vbs
ping -n %delay% 127.0.0.1 > nul
date %current_date%
exit

