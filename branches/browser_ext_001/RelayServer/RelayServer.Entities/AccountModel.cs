using System;
using System.Runtime.Serialization;

namespace RelayServer.Entities
{
	[DataContract]
	public class AccountModel
	{
		[DataMember]
		public string Name
		{
			get;
			set;
		}

		[DataMember]
		public string Password
		{
			get;
			set;
		}

		[DataMember]
		public bool IsOccupied
		{
			get;
			set;
		}

		[DataMember]
		public System.DateTime? SessionTime
		{
			get;
			set;
		}
	}
}
