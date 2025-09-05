using Connectors.IB;
using GreatOptionTrader.ViewModels;
using System;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для OpenMasterPositionVIew.xaml
/// </summary>
public partial class OpenMasterPositionVIew : Window {
    private readonly InteractiveBroker broker;
    private readonly OptionStrategyContainerViewModel container;

    public OrderParamsViewModel OrderParams { get; private set; }

    public OpenMasterPositionVIew (InteractiveBroker broker, OptionStrategyContainerViewModel container) {
        InitializeComponent();
        this.broker = broker;
        this.container = container;

        OrderParams = new OrderParamsViewModel() {
            Price = container.LastPrice,
            Volume = 1,
            Direction = Core.TradeDirection.Buy,
        };

        ugOrderParams.DataContext = OrderParams;
    }

    private void sendMasterOrder (object sender, RoutedEventArgs e) {
        if (OrderParams.Price == 0m) return;
        try {
            var order = container.MakeOrder(broker, container.Container.Account, OrderParams);
            broker.PlaceOrder(container.Instrument, order);
        }
        catch (Exception exception) {
            MessageBox.Show(exception.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }   
}
