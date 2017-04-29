namespace NewLauncher.Helper
{
    using NewLauncher.DataContext;
    using System;
    using System.Net;
    using System.Reflection;

    public class ErrorLogHelper
    {
        public static void AddErrorInLog(string NameError, string DescrError)
        {
            try
            {
                using (AvtoritetEntities entities = new AvtoritetEntities())
                {
                    LogErrorsSet entity = new LogErrorsSet();
                    string hostName = Dns.GetHostName();
                    string str2 = "";
                    for (int i = 0; i < Dns.GetHostEntry(hostName).AddressList.Length; i++)
                    {
                        if (!Dns.GetHostEntry(hostName).AddressList[i].IsIPv6LinkLocal)
                        {
                            if (str2 != "")
                            {
                                str2 = str2 + ", ";
                            }
                            str2 = str2 + Dns.GetHostEntry(hostName).AddressList[i].ToString();
                        }
                    }
                    string userName = Environment.UserName;
                    string machineName = Environment.MachineName;
                    entity.Computer = hostName + ", " + str2 + ", " + userName + ", " + machineName;
                    entity.DateError = new DateTime?(DateTime.Now);
                    entity.ExeName = "Лаунчер";
                    entity.NameError = NameError;
                    entity.DescrError = DescrError;
                    entities.LogErrorsSet.Add(entity);
                    entities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MainWindow.Logging(e);
            }
        }
    }
}

