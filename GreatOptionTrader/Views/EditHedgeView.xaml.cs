using Connectors.IB;
using Core.Types;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels;
using System;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для EditHedgeView.xaml
/// </summary>
public partial class EditHedgeView : Window {
    private readonly InteractiveBroker broker;
    private readonly OptionStrategyContainerViewModel container;

    public OrderParamsViewModel OrderParams { get; private set; }
    
    public EditHedgeView (InteractiveBroker broker, OptionStrategyContainerViewModel container) {
        InitializeComponent();

        OrderParams = new OrderParamsViewModel() {
            Price = container.LastPrice,
            Volume = 1,
            Direction = TradeDirection.Buy,
        };

        ugOrderParams.DataContext = OrderParams;
        this.broker = broker;
        this.container = container;
        Title = this.container.Instrument.Name;
        lvHedges.ItemsSource = this.container.OptionStrategies;
    }

    private void sendOrder (object sender, RoutedEventArgs e) {
        if (OrderParams.Price == 0m) return;
        if (lvHedges.SelectedItem is not OptionStrategyViewModel strategy) {
            return;
        }

        try {
            strategy.MakeAndPlaceOrder(broker, container.Container.Account, OrderParams);
        }
        catch (Exception exception) {
            MessageBox.Show(exception.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void requestNewHedge(object sender, RoutedEventArgs e)
    {
        if (!broker.IsConnected())
        {
            MessageBox.Show(
                "Необходимо подкллючитья к торговой площщадке!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var dialog = new RequestOptionView(broker);
        if (dialog.ShowDialog() == true && dialog.SelectedOption != null)
        {
            var strategy = new OptionStrategy()
            {
                Instrument = dialog.SelectedOption,
                Orders = []
            };

            container.AddStrategy(strategy);
        }
    }
}
