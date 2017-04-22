@echo off
cd "C:\honda eu\soft"
start regedit /s C:\Scripts\rus.reg

start regedit /s Eu.reg

set old_date=%DATE%

date 20-04-2011


epc.exe



rem reg delete HKEY_LOCAL_MACHINE\SOFTWARE\Honda /f

date %old_date%

exit