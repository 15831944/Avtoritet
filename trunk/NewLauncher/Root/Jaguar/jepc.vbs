Set objShell=CreateObject("WScript.Shell")
Wscript.Sleep 500
objShell.SendKeys "admin",True
Wscript.Sleep 300
objShell.SendKeys "{TAB}",True
Wscript.Sleep 300
objShell.SendKeys "admin",True
objShell.SendKeys "{ENTER}",True
