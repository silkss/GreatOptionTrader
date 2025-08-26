using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GreatOptionTrader.Views.Converters;
internal class IsStartedToBrush : IValueConverter {
    
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is not bool b) throw new NotImplementedException();
        return b ? Brushes.LightGreen : Brushes.Black;
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
