Set objShell=CreateObject("WScript.Shell")
Wscript.Sleep 1000
Do Until Success = True
    Success = objShell.AppActivate("DEU")
    Wscript.Sleep 1000
Loop

objShell.AppActivate("DEU")
Wscript.Sleep 200
objShell.SendKeys "^l",True
Wscript.Sleep 300
objShell.SendKeys "user",True
Wscript.Sleep 300
objShell.SendKeys "{TAB}",True
Wscript.Sleep 300
objShell.SendKeys "pass",True
objShell.SendKeys "{ENTER}",True
