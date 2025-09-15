using Connectors.IB;
using GreatOptionTrader.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.TextFormatting;

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
        tbTargetPnL.SetBinding(
            TextBox.TextProperty,
            new Binding() {
                Source = container.ContainerSettings,
                Path = new PropertyPath(nameof(container.ContainerSettings.CurrencyTargetPnL)),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
            });

        ugOrderParams.DataContext = OrderParams;
    }

    private void sendMasterOrder (object sender, RoutedEventArgs e) {
        if (OrderParams.Price == 0m) return;
        try {
            container.MakeAndPlaceOrder(broker, container.Container.Account, OrderParams);
        }
        catch (Exception exception) {
            MessageBox.Show(exception.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }   
}
