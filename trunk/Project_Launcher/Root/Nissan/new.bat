@echo off
setlocal ENABLEDELAYEDEXPANSION
set delay=15
start regedit /s C:\Scripts\jap.reg 
copy nsfastky.ini C:\NISSAN\nsfastky.ini
echo Dont close this window! It will be close automaticly.
start C:\WINDOWS\AppPatch\AppLoc.exe "C:\NISSAN\Nfmenu.exe" "/L0411"
:TEST
@ping -n %delay% 127.0.0.1 > nul
tasklist | Find /i "nfmenu.exe" || (goto Else)
:THEN
Goto test
:ELSE
start regedit /s C:\Scripts\rus.reg 
exit
