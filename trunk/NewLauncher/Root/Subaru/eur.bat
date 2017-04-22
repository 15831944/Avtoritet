@echo off
cd C:\SUBARUEX2\
start regedit /s C:\Scripts\rus.reg
copy /y C:\SUBARUEX2\LH\SFFASTEW.INI C:\SUBARUEX2\SFFASTEW.INI>nul
.\subaru.exe
exit
