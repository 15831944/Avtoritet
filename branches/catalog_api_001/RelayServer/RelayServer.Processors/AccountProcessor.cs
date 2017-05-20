using RelayServer.Entities;
using RelayServer.Helpers;
using RelayServer.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;

namespace RelayServer.Processors
{
	public class AccountProcessor
	{
		private readonly System.Collections.Generic.List<AccountModel> userAccounts = new System.Collections.Generic.List<AccountModel>();

		private static AccountProcessor instance;

		public static AccountProcessor Instance
		{
			get
			{
				AccountProcessor arg_15_0;
				if ((arg_15_0 = AccountProcessor.instance) == null)
				{
					arg_15_0 = (AccountProcessor.instance = new AccountProcessor());
				}
				return arg_15_0;
			}
		}

        private TimeSpan AccountSessionTimeout;

        private AccountProcessor()
		{
            AccountSessionTimeout = TimeSpan.FromHours(int.Parse(ConfigurationManager.AppSettings["AccountSessionTimeout"]));

            Timer timer = new Timer
			{
				Interval = 60000.0,
				AutoReset = true
			};
			timer.Elapsed += new ElapsedEventHandler(this.SessionTimerOnElapsed);
			timer.Start();
		}

		private void SessionTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			System.Collections.Generic.IEnumerable<AccountModel> accounts = from x in this.userAccounts
			    where x.SessionTime.HasValue && (System.DateTime.Now - x.SessionTime).Value.TotalHours >= (int)AccountSessionTimeout.TotalHours
                select x;
			foreach (AccountModel accountModel in accounts)
			{
				accountModel.IsOccupied = false;
				accountModel.SessionTime = null;
				ConsoleHelper.Info(string.Format("Account: {0} has released", accountModel.Name));
			}
		}

		public void Initialize()
		{
			string[] accounts = Resources.Accounts.Replace("\r\n", string.Empty).Split(new char[]
			{
				';'
			});
			foreach (string[] buffer2 in from account in accounts
			select account.Split(new char[]
			{
				':'
			}) into buffer
			where buffer.Length > 1
			select buffer)
			{
				this.userAccounts.Add(new AccountModel
				{
					Name = buffer2[0],
					Password = buffer2[1],
					IsOccupied = false
				});
			}
		}

		public void FreeOccupiedAccount(string loginName)
		{
			if (!string.IsNullOrEmpty(loginName))
			{
				AccountProcessor.UpdateOccupiedAccount(this.userAccounts.Find((AccountModel x) => x.Name.Equals(loginName, System.StringComparison.InvariantCultureIgnoreCase)));
				ConsoleHelper.Info(string.Format("Account: {0} has released", loginName));
				ConsoleHelper.Info(string.Format("Free accounts: {0}. Occupied accounts: {1}", this.userAccounts.Count((AccountModel x) => !x.IsOccupied), this.userAccounts.Count((AccountModel x) => x.IsOccupied)));
			}
		}

		public AccountModel GetUnoccupiedAccount()
		{
			AccountModel accountModel = this.userAccounts.FirstOrDefault((AccountModel x) => !x.IsOccupied);
			AccountModel result;
			if (accountModel == null)
			{
				ConsoleHelper.Error("Not found any account");
				result = null;
			}
			else
			{
				accountModel.IsOccupied = true;
				accountModel.SessionTime = new System.DateTime?(System.DateTime.Now);
				ConsoleHelper.Info(string.Format("Account: {0} has taken", accountModel.Name));
				ConsoleHelper.Info(string.Format("Free accounts {0}. Occupied accounts {1}", this.userAccounts.Count((AccountModel x) => !x.IsOccupied), this.userAccounts.Count((AccountModel x) => x.IsOccupied)));
				result = accountModel;
			}
			return result;
		}

		private static void UpdateOccupiedAccount(AccountModel accountModel)
		{
			accountModel.IsOccupied = false;
			accountModel.SessionTime = null;
		}
	}
}
