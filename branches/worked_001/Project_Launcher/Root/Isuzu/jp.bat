@echo off
setlocal ENABLEDELAYEDEXPANSION
set delay=20
start regedit /s C:\Scripts\jap.reg 


C:\WINDOWS\AppPatch\AppLoc.exe "C:\ISUZU\PASS3\PassSys\PassWin.exe" "/L0411"
:TEST
@ping -n %delay% 127.0.0.1 > nul
tasklist | Find /i "PassWin.exe" || (goto Else)
:THEN
Goto test
:ELSE
start regedit /s C:\Scripts\rus.reg 

exit