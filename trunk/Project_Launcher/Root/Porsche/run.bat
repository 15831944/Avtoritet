@echo off
start 	\\avtoritet24.com\epc\apps\Pet7\Pet7.exe
ping 127.0.0.1 -n 2 > nul
start 	\\avtoritet24.com\epc\apps\Pet7\Pet7.exe
exit
cd C:\PET\PROG
start C:\PET\PROG\pet73app.exe C:\PET\PROG\ETKA_PO.INI
cd C:\Debug.Launcher\Root\Porsche
start pet.vbs
exit