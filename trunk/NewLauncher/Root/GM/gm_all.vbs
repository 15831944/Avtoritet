Set objShell=CreateObject("WScript.Shell")
dim curdate, tempdate, objShell
rem Option Explicit
rem Dim objShell
rem On Error Resume Next

rem Const OverwriteExisting = True
rem Set objFSO = CreateObject("Scripting.FileSystemObject")
rem objFSO.CopyFile "C:\MCHYW\DeMoN\mcprefs.dat" , "C:\Documents and Settings\All Users\Application Data\MCADMIN\HYW\", OverwriteExisting

rem Set WshShell = CreateObject("WScript.Shell")
rem objShell.RegWrite "HKEY_CURRENT_USER\Software\VB and VBA Program Settings\CommInH\UNH\TOP", "1-3-2015-0-0-8", "REG_SZ"

rem curdate = DatePart("d",Now)&"-"&DatePart("m",Now)&"-"&DatePart("YYYY",Now)
rem tempdate = "01.03.2015"
rem set_tempdate = "%comspec% /c date " & tempdate
rem set_curdate =  "%comspec% /c date " & curdate
rem objShell.Run (set_tempdate)

Wscript.Sleep 1000
Do Until Success = True
    Success = objShell.AppActivate("GMC")
    Wscript.Sleep 1000
Loop

objShell.AppActivate("GMC")
Wscript.Sleep 200
objShell.SendKeys "^l",True
Wscript.Sleep 500
objShell.SendKeys "1",True
objShell.SendKeys "{TAB}",True
Wscript.Sleep 500
objShell.SendKeys "1",True
objShell.SendKeys "{ENTER}",True

rem objShell.Run (set_curdate)
Set objShell=Nothing