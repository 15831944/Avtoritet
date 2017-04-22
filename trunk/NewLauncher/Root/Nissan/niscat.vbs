Set objShell=CreateObject("WScript.Shell")


objShell.AppActivate("Niscat")
Wscript.Sleep 200
objShell.SendKeys "{ENTER}",True
Wscript.Sleep 200
objShell.SendKeys "{ENTER}",True