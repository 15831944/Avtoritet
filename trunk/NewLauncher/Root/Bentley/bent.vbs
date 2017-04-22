Set objShell=CreateObject("WScript.Shell")
dim curdate, tempdate, objShell
curdate = DatePart("d",Now)&"-"&DatePart("m",Now)&"-"&DatePart("YYYY",Now)
tempdate = "16.03.2010"
set_tempdate = "%comspec% /c date " & tempdate
set_curdate =  "%comspec% /c date " & curdate
objShell.Run (set_tempdate)

objshell.Run "C:\IETIS\HTView.exe"
Wscript.Sleep 8000

objShell.Run (set_curdate)
Set objShell=Nothing