@echo off
cd C:\SUBARUEX2\
start regedit /s c:\Scripts\rus.reg
copy /y C:\SUBARUEX2\USA\SFFASTEW.INI C:\SUBARUEX2\SFFASTEW.INI>nul
.\subaru.exe
exit
