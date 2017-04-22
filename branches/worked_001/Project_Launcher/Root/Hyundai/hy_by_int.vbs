Set objShell=CreateObject("WScript.Shell")
dim curdate, tempdate, objShell
rem Option Explicit
rem Dim objShell
rem On Error Resume Next

Const OverwriteExisting = True
Set objFSO = CreateObject("Scripting.FileSystemObject")
objFSO.CopyFile "C:\Debug.Launcher\Root\Hyundai\mcprefs.dat" , "C:\Documents and Settings\All Users\Application Data\MCADMIN\HYW\", OverwriteExisting

Set WshShell = CreateObject("WScript.Shell")
objShell.RegWrite "HKEY_CURRENT_USER\Software\VB and VBA Program Settings\CommInH\UNH\TOP", "0-2-2016-0-0-8", "REG_SZ"

curdate = DatePart("d",Now)&"-"&DatePart("m",Now)&"-"&DatePart("YYYY",Now)
tempdate = "20.02.2016"
set_tempdate = "%comspec% /c date " & tempdate
set_curdate =  "%comspec% /c date " & curdate
objShell.Run (set_tempdate)


objshell.Run "C:\MCHYW\MCHYW.EXE"
Wscript.Sleep 5000
Do Until Success = True
    Success = objShell.AppActivate("HYW")
    Wscript.Sleep 1000
Loop

objShell.AppActivate("Microcat")
objShell.AppActivate("HYW")
Wscript.Sleep 200
objShell.SendKeys "^l",True

rem objShell.SendKeys "{ENTER}",True
Wscript.Sleep 500
objShell.SendKeys "user",True
objShell.SendKeys "{TAB}",True
Wscript.Sleep 500
objShell.SendKeys "pass",True
objShell.SendKeys "{ENTER}",True

objShell.Run (set_curdate)
Set objShell=Nothing