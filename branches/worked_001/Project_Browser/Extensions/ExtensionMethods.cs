using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrowserExtension.Extensions
{
	public static class ExtensionMethods
	{
		public static ObservableCollection<T> ToCollection<T>(this IEnumerable<T> enumerable)
		{
			return new ObservableCollection<T>(enumerable);
		}
	}
}
