using Connectors.IB;
using GreatOptionTrader.Commands;
using GreatOptionTrader.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для EditInstrumentGroupView.xaml
/// </summary>
public partial class EditInstrumentGroupView : Window {
    public EditInstrumentGroupView (
        OptionStrategyContainerViewModel vm, 
        InteractiveBroker broker) {
        InitializeComponent();
        DataContext = vm;
        cbAccounts.ItemsSource = broker.Accounts;
        tbnCancelOrder.Command = new CancelOrderCommand(broker);
        btnRequest.Command = new RequestOptionCommand(broker);
        btnSendOrder.Command = new SendOrderCommand(broker);
    }

    protected override void OnClosing (CancelEventArgs e) {
        base.OnClosing(e);
    }
}
