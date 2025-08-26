using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GreatOptionTrader.Views.Converters;
public class PnLToBrush : IValueConverter {
    private readonly static LinearGradientBrush negativePnlColor = new(
            Colors.Transparent,
            Color.FromRgb(246, 130, 140),
            new System.Windows.Point(0, .5),
            new System.Windows.Point(1.5, .5));

    private readonly static LinearGradientBrush positivePnlColor = new(
        Colors.Transparent,
        Color.FromRgb(200, 214, 100),
        new System.Windows.Point(0, .5),
        new System.Windows.Point(1.5, .5));

    public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is null) return negativePnlColor;
        if ((decimal)value == 0) return negativePnlColor;
        return (decimal)value <= 0 ? negativePnlColor : positivePnlColor;
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
