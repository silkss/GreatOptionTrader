using GreatOptionTrader.Commands;
using GreatOptionTrader.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для EditInstrumentGroupView.xaml
/// </summary>
public partial class EditInstrumentGroupView : Window {
    public EditInstrumentGroupView (
        GroupViewModel vm, 
        ObservableCollection<string> accounts,
        SendOrderCommand sendOrderCommand,
        CancelOrderCommand cancelOrderCommand,
        RequestOptionCommand requestOptionCommand) {
        InitializeComponent();
        DataContext = vm;
        cbAccounts.ItemsSource = accounts;
        tbnCancelOrder.Command = cancelOrderCommand;
        btnRequest.Command = requestOptionCommand;
        btnSendOrder.Command = sendOrderCommand;
    }

    protected override void OnClosing (CancelEventArgs e) {
        base.OnClosing(e);
    }
}
