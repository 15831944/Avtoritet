@echo off
setlocal ENABLEDELAYEDEXPANSION
set delay=20
copy C:\mazdadata\launch\Japan\MAZEPC.INI c:\windows\
start regedit /s C:\Scripts\jap.reg 
echo Dont close this window! It will be close automaticly.
C:\WINDOWS\AppPatch\AppLoc.exe "C:\MAZEPCJp\BIN\mazdaepc.exe" "/L0411"
:TEST
@ping -n %delay% 127.0.0.1 > nul
tasklist | Find /i "mazdaepc.exe" || (goto Else)
:THEN
Goto test
:ELSE
start regedit /s C:\Scripts\rus.reg
exit