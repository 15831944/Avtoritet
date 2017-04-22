Set objShell=CreateObject("WScript.Shell")
dim curdate, tempdate, objShell

Const OverwriteExisting = True
Set objFSO = CreateObject("Scripting.FileSystemObject")
objFSO.CopyFile "C:\MCFNA\DeMoN\mcprefs.dat" , "C:\Documents and Settings\All Users\Application Data\MCADMIN\FNA\", OverwriteExisting

curdate = DatePart("d",Now)&"-"&DatePart("m",Now)&"-"&DatePart("YYYY",Now)
'tempdate = "12-12-2014"
tempdate = "01-01-2015"
set_tempdate = "%comspec% /c date " & tempdate
set_curdate = "%comspec% /c date " & curdate
objShell.Run (set_tempdate)

objshell.Run "C:\mcfna\mcfna.EXE"
Wscript.Sleep 5000
Do Until Success = True
    Success = objShell.AppActivate("FNA")
    Wscript.Sleep 1000
Loop

objShell.AppActivate("FNA")
Wscript.Sleep 500
'objShell.SendKeys "^l",True

objShell.SendKeys "{ENTER}",True
Wscript.Sleep 500
objShell.SendKeys "user",True
objShell.SendKeys "{TAB}",True
Wscript.Sleep 500
objShell.SendKeys "pass",True
objShell.SendKeys "{ENTER}",True
Wscript.Sleep 500
objShell.Run (set_curdate)
Set objShell=Nothing