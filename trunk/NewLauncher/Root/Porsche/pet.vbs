Set objShell=CreateObject("WScript.Shell")
Wscript.Sleep 1000
Do Until Success = True
    Success = objShell.AppActivate("PET")
    Wscript.Sleep 500
Loop

objShell.AppActivate("PET")
Wscript.Sleep 500
objShell.SendKeys "{ENTER}",True
