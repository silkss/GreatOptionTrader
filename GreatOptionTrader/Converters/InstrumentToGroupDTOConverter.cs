using GreatOptionTrader.DTO;
using GreatOptionTrader.Models;
using System;
using System.Windows.Data;
using System.Globalization;

namespace GreatOptionTrader.Converters;

internal class InstrumentToGroupDTOConverter : IMultiValueConverter {
    public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture) {
        if (values[0] is not InstrumentGroup group) {
            return new();
        }
        if (values[1] is not string name) {
            return new();
        }
        if (values[2] is not string exchange) {
            return new();
        }
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(exchange)) {
            return new();
        }
        return new InstrumentToGroupDTO() {
            Group = group,
            InstrumentRequestDTO = new InstrumentRequestDTO() {
                InstrumentName = name,
                InstrumentExchange = exchange,
            }
        };
    }

    public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
