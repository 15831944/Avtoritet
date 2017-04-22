using RelayServer.DataContext;
using System;
using System.Net;

namespace RelayServer.Helpers
{
	internal class ErrorLogHelper
	{
		public static void AddErrorInLog(string NameError, string DescrError)
		{
			try
			{
				using (AvtoritetEntities ae = new AvtoritetEntities())
				{
					LogErrors log = new LogErrors();
					string myHost = Dns.GetHostName();
					string compIP = "";
					for (int i = 0; i < Dns.GetHostEntry(myHost).AddressList.Length; i++)
					{
						if (!Dns.GetHostEntry(myHost).AddressList[i].IsIPv6LinkLocal)
						{
							if (compIP != "")
							{
								compIP += ", ";
							}
							compIP += Dns.GetHostEntry(myHost).AddressList[i].ToString();
						}
					}
					string userNameWin = System.Environment.UserName;
					string compName = System.Environment.MachineName;
					log.Computer = string.Concat(new string[]
					{
						myHost,
						", ",
						compIP,
						", ",
						userNameWin,
						", ",
						compName
					});
					log.DateError = new System.DateTime?(System.DateTime.Now);
					log.ExeName = "Прокси сервер";
					log.NameError = NameError;
					log.DescrError = DescrError;
					ae.LogErrors.Add(log);
					ae.SaveChanges();
				}
			}
			catch
			{
			}
		}
	}
}
