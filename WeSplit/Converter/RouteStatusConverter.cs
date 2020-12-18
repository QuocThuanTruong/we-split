using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WeSplit.Converter
{
	class RouteStatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isDone = true;
			int passedValue = (int)value;

			if (passedValue == 0)
			{
				isDone = false;
			}	
			else
			{
				isDone = true;
			}	
			
			Debug.WriteLine(isDone);

			return isDone;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
